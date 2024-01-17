namespace SystemBase;

public sealed record Success : ICQRSResponse
{
    public static readonly Success Instance = new();
}