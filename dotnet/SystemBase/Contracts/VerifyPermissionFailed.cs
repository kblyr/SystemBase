namespace SystemBase;

public sealed record VerifyPermissionFailed : ICQRSErrorResponse
{
    public int PermissionId { get; init; }
}