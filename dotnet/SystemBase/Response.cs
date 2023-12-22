namespace SystemBase;

public interface ICQRSResponse {}

public interface ICQRSErrorResponse : ICQRSResponse {}

public sealed record Success : ICQRSResponse
{
    public static readonly Success Instance = new();
}

public sealed record Failed : ICQRSErrorResponse
{
    public static readonly Failed Instance = new();
}

public sealed record VerifyPermissionFailed : ICQRSErrorResponse
{
    public int PermissionId { get; init; }
}