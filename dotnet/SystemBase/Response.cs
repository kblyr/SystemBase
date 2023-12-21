namespace SystemBase;

public interface ICQRSResponse {}

public interface ICQRSErrorResponse : ICQRSResponse {}

public sealed record Success : ICQRSResponse {}

public sealed record Error : ICQRSErrorResponse {}

public sealed record VerifyPermissionFailed : ICQRSErrorResponse
{
    public int PermissionId { get; init; }
}