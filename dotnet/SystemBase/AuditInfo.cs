namespace SystemBase;

public sealed record AuditInfo
{
    public int UserId { get; init; }
    public DateTimeOffset Timestamp { get; init; }
}

public interface ICurrentAuditInfoProvider 
{
    AuditInfo Value { get; }
}