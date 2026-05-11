namespace HuiTrade.Domain.Aggregates;

public class UserAddress
{
    public Guid Id {get; private set; }
    public Guid UserId {get; private set;}
    public string ReceiverName { get; private set; } = null!;
    public string Phone { get; private set; } = null!;
    public string DetailAddress { get; private set; } = null!;
    public bool IsDefault { get; private set; }
    public DateTime CreatedAt { get; private set; }
    
    private  UserAddress() {}

    internal UserAddress(Guid userId, string receiverName, string phone, string detail, bool isDefault)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        ReceiverName = receiverName;
        Phone = phone;
        DetailAddress = detail;
        IsDefault = isDefault;
        CreatedAt = DateTime.UtcNow;
    }
    internal void UpdateDefaultState(bool isDefault)
    {
        IsDefault = isDefault;
         
    }
}