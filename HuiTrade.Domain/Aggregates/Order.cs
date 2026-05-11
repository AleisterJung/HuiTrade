namespace HuiTrade.Domain.Aggregates;

public class Order
{
    public Guid Id { get; private set; }
    public String OrderSn { get; private set; } = null!;
    public Guid ProductId { get; private set; }
    public Guid BuyerId { get; private set; }
    public Guid SellerId { get; private set; }
    
    public decimal Amount { get; private set; }

    public OrderStatus Status { get; private set; }
    //区块链 交易Hash
    public String? TxHash {get; private set; }
    
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    public Order (){}


    public WalletTransaction Lock(User buyer, string productName)
    {
        if (buyer.Id != this.BuyerId) throw new Exception("越权操作");
        if (Status != OrderStatus.Created) throw new InvalidOperationException("当前状态不允许锁定");

        // 接收 User 返回的流水对象
        var transaction = buyer.PlaceOrderPay(this.Id, this.Amount, productName);

        this.Status = OrderStatus.Locked;
        this.UpdatedAt = DateTime.UtcNow;

        return transaction;
    }

    /// <summary>
    /// 完成订单：返回买家结算和卖家入账的两笔流水
    /// </summary>
    public (WalletTransaction buyerTx, WalletTransaction sellerTx) Complete(User buyer, User seller)
    {
        if (this.Status != OrderStatus.Locked) throw new InvalidOperationException("订单尚未锁定，无法结算");
        if (buyer.Id != this.BuyerId || seller.Id != this.SellerId) throw new InvalidOperationException("交易双方身份不匹配");

        // 1. 买家核销冻结资金
        var buyerTx = buyer.ConfirmOrderSettle(this.Id, this.Amount);

        // 2. 卖家收到货款
        var sellerTx = seller.ReceiveTradeSettlement(this.Id, this.Amount);

        this.Status = OrderStatus.Released;
        this.UpdatedAt = DateTime.UtcNow;

        return (buyerTx, sellerTx);
    }

    /// <summary>
    /// 取消订单：如果是已锁定状态，返回退款流水
    /// </summary>
    public WalletTransaction? Cancel(User buyer)
    {
        if (buyer.Id != this.BuyerId) throw new Exception("越权操作");
        if (this.Status != OrderStatus.Created && this.Status != OrderStatus.Locked)
            throw new InvalidOperationException("当前状态不可取消订单");

        WalletTransaction? refundTx = null;

        if (this.Status == OrderStatus.Locked)
        {
            // 接收退款产生的流水
            refundTx = buyer.CancelOrderRefund(this.Id, this.Amount);
        }

        this.Status = OrderStatus.Cancelled;
        this.UpdatedAt = DateTime.UtcNow;

        return refundTx;
    }
}