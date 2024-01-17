namespace SystemBase;

public sealed record PermissionNotFound : ICQRSErrorResponse
{
    public int Id { get; init; }
}