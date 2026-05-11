namespace HuiTrade.Domain.Aggregates;

public class UserWallet
{
    public Guid UserId { get; private set; }
    public User User { get; set; }
    public decimal Balance { get; private set; }        // 可用余额
    public decimal FrozenBalance { get; private set; }  // 冻结余额

    public long Version { get; private set; }           // 乐观锁

    public DateTime UpdatedAt { get; private set; }

    private UserWallet() { }

    public WalletChange Freeze(decimal amount)
    {
        if (amount <= 0) throw new Exception("invalid amount");
        if (Balance < amount) throw new Exception("insufficient balance");

        var before = Snapshot();

        Balance -= amount;
        FrozenBalance += amount;

        UpdatedAt = DateTime.UtcNow;
        Version++;

        return SnapshotDiff(before);
    }

    public WalletChange Unfreeze(decimal amount)
    {
        if (FrozenBalance < amount) throw new Exception("insufficient frozen");

        var before = Snapshot();

        FrozenBalance -= amount;
        Balance += amount;

        UpdatedAt = DateTime.UtcNow;
        Version++;

        return SnapshotDiff(before);
    }

    public WalletChange Settle(decimal amount)
    {
        if (FrozenBalance < amount) throw new Exception("insufficient frozen");

        var before = Snapshot();

        FrozenBalance -= amount;

        UpdatedAt = DateTime.UtcNow;
        Version++;

        return SnapshotDiff(before);
    }

    public WalletChange Credit(decimal amount)
    {
        var before = Snapshot();

        Balance += amount;

        UpdatedAt = DateTime.UtcNow;
        Version++;

        return SnapshotDiff(before);
    }

    public WalletChange Debit(decimal amount)
    {
        if (Balance < amount) throw new Exception("insufficient balance");

        var before = Snapshot();

        Balance -= amount;

        UpdatedAt = DateTime.UtcNow;
        Version++;

        return SnapshotDiff(before);
    }

    private WalletSnapshot Snapshot() =>
        new(Balance, FrozenBalance);

    private WalletChange SnapshotDiff(WalletSnapshot before) =>
        new(before.Balance, Balance, before.Frozen, FrozenBalance);

    internal UserWallet(Guid userId, decimal initialBalance)
    {
        UserId = userId;
        Balance = initialBalance;
        FrozenBalance = 0m;
        UpdatedAt = DateTime.UtcNow;
        Version = 1; // 初始版本
    }
}

public record WalletChange(
    decimal BeforeBalance,
    decimal AfterBalance,
    decimal BeforeFrozen,
    decimal AfterFrozen
);

public record WalletSnapshot(decimal Balance, decimal Frozen);

