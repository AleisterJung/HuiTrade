using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace HuiTrade.Domain.Aggregates;

public class User
{
    // --- 基础属性 ---
    public Guid Id { get; private set; }
    public string Username { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
    public string Nickname { get; private set; } = string.Empty;
    public string? Email { get; private set; }
    public string? WalletAddress { get; private set; }
    public string AvatarUrl { get; private set; } = string.Empty;
    public bool IsVerified { get; private set; }
    public DateTime CreatedAt { get; private set; }

    // --- 导航属性 ---
    // 核心修复：只保留这一个 UserWallet，删除了之前冲突的 Wallet 属性
    public virtual UserWallet UserWallet { get; private set; } = null!;

    private readonly List<UserAddress> _addresses = new();
    public virtual IReadOnlyCollection<UserAddress> Addresses => _addresses.AsReadOnly();

    // EF Core 需要的私有构造函数
    private User() { }

    // --- 领域方法 ---

    /// <summary>
    /// 注册新用户并初始化钱包
    /// </summary>
    public static User Register(string username, string passwordHash, string nickname, string? email = null)
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            Username = username,
            PasswordHash = passwordHash,
            Nickname = nickname,
            CreatedAt = DateTime.UtcNow,
            IsVerified = false,
            AvatarUrl = "" // 默认头像
        };

        // 初始化关联的钱包，确保使用共享主键 (Shared Primary Key)
        user.UserWallet = new UserWallet(user.Id, 1000m);
        return user;
    }

    /// <summary>
    /// 下单支付：冻结余额
    /// </summary>
    public WalletTransaction PlaceOrderPay(Guid orderId, decimal amount, string productName)
    {
        // 统一使用 UserWallet 属性名
        var change = this.UserWallet.Freeze(amount);
        return WalletTransaction.Create(this.Id, orderId, TransactionType.OrderLock, TransactionDirection.Out, amount, change, $"购买: {productName}");
    }

    /// <summary>
    /// 确认收货：结算扣除冻结款
    /// </summary>
    public WalletTransaction ConfirmOrderSettle(Guid orderId, decimal amount)
    {
        var change = this.UserWallet.Settle(amount);
        return WalletTransaction.Create(this.Id, orderId, TransactionType.BuyerSettle, TransactionDirection.Out, amount, change, "买家结算");
    }

    /// <summary>
    /// 订单取消：退款（解冻）
    /// </summary>
    public WalletTransaction CancelOrderRefund(Guid orderId, decimal amount)
    {
        var change = this.UserWallet.Unfreeze(amount);
        return WalletTransaction.Create(this.Id, orderId, TransactionType.OrderLock, TransactionDirection.In, amount, change, "订单退款");
    }

    /// <summary>
    /// 收到贸易款项
    /// </summary>
    public WalletTransaction ReceiveTradeSettlement(Guid orderId, decimal amount)
    {
        var change = this.UserWallet.Credit(amount);
        return WalletTransaction.Create(this.Id, orderId, TransactionType.SellerIncome, TransactionDirection.In, amount, change, "卖家收钱");
    }

    /// <summary>
    /// 添加收货地址
    /// </summary>
    public void AddAddress(string receiverName, string phone, string detail, bool isDefault = false)
    {
        if (isDefault)
        {
            foreach (var addr in _addresses)
            {
                addr.UpdateDefaultState(false);
            }
        }
        var newAddress = new UserAddress(this.Id, receiverName, phone, detail, isDefault);
        _addresses.Add(newAddress);
    }
}