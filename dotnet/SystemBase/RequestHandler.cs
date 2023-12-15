namespace SystemBase;

public interface ICQRSRequestHandler<T> : IRequestHandler<T, ICQRSResponse> where T : ICQRSRequest {}