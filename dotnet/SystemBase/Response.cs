namespace SystemBase;

public interface ICQRSResponse {}

public interface ICQRSErrorResponse : ICQRSResponse {}

public sealed record Success : ICQRSResponse
{
    public static readonly Success Instance = new();
}

public sealed record Error : ICQRSErrorResponse
{
    public static readonly Error Instance = new();
}

public sealed record VerifyPermissionFailed : ICQRSErrorResponse
{
    public int PermissionId { get; init; }
}