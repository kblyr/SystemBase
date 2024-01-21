namespace SystemBase;

public sealed record Failed : ICQRSErrorResponse
{
    public static readonly Failed Instance = new();
}