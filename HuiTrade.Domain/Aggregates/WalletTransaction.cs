namespace HuiTrade.Domain.Aggregates;

public class WalletTransaction
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public Guid? RelatedOrderId { get; private set; }
    public TransactionType Type { get; private set; }
    public TransactionDirection Direction { get; private set; }
    public decimal Amount { get; private set; }
    public decimal BeforeBalance { get; private set; }
    public decimal AfterBalance { get; private set; }
    public string Description { get; private set; } = null!;
    public DateTime CreatedAt { get; private set; }

    private WalletTransaction() { }

    /// <summary>
    /// 唯一正确的创建方式：接收由 Wallet 产生的物理真相 (WalletChange)
    /// </summary>
    public static WalletTransaction Create(
        Guid userId,
        Guid? orderId,
        TransactionType type,
        TransactionDirection direction,
        decimal amount,
        WalletChange change, // 接收快照
        string description)
    {
        // 自动识别：BuyerSettle 记录冻结池快照，其他记录可用余额快照
        decimal before = (type == TransactionType.BuyerSettle) ? change.BeforeFrozen : change.BeforeBalance;
        decimal after = (type == TransactionType.BuyerSettle) ? change.AfterFrozen : change.AfterBalance;

        return new WalletTransaction
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            RelatedOrderId = orderId,
            Type = type,
            Direction = direction,
            Amount = amount,
            BeforeBalance = before,
            AfterBalance = after,
            Description = description,
            CreatedAt = DateTime.UtcNow
        };
    }
}