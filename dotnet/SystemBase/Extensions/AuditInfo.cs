namespace SystemBase;

public static class AuditInfoExtensions
{
    public static DateTimeOffset Timestamp(this AuditInfo info) => info.Get<DateTimeOffset>(AuditInfo.Keys.Timestamp);
    public static int? UserId(this AuditInfo info) => info.Get<int?>(AuditInfo.Keys.UserId);

    // Cached
    public static DateTimeOffset Timestamp(this CachedAuditInfo info) => info.Get<DateTimeOffset>(AuditInfo.Keys.Timestamp);
    public static int? UserId(this CachedAuditInfo info) => info.Get<int?>(AuditInfo.Keys.UserId);
}