using System.Security.Claims;

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

public interface IAuditInfoExtractor
{
    AuditInfo Extract(ClaimsPrincipal principal);
}