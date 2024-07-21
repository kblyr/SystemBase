namespace SystemBase;

public interface IAPIMediator
{
    Task<ICQRSResponse> Send<TRequestFrom, TRequestTo>(TRequestFrom requestFrom, CancellationToken cancellationToken = default)
        where TRequestFrom : IAPIRequest
        where TRequestTo : ICQRSRequest;

    Task<ICQRSResponse> Send<TRequestFrom, TRequestTo>(TRequestFrom requestFrom, MutateRequest<TRequestTo> mutateRequest, CancellationToken cancellationToken = default)
        where TRequestFrom : IAPIRequest
        where TRequestTo : ICQRSRequest;

    Task<IActionResult> SendThenMap<TRequestFrom, TRequestTo>(TRequestFrom requestFrom, CancellationToken cancellationToken = default)
        where TRequestFrom : IAPIRequest
        where TRequestTo : ICQRSRequest;

    Task<IActionResult> SendThenMap<TRequestFrom, TRequestTo>(TRequestFrom requestFrom, MutateRequest<TRequestTo> mutateRequest, CancellationToken cancellationToken = default)
        where TRequestFrom : IAPIRequest
        where TRequestTo : ICQRSRequest;
}

public delegate T MutateRequest<T>(T request);

sealed class APIMediator(MediatR.IMediator mediator, MapsterMapper.IMapper mapper, IResponseMapper responseMapper) : IAPIMediator
{
    public async Task<ICQRSResponse> Send<TRequestFrom, TRequestTo>(TRequestFrom requestFrom, CancellationToken cancellationToken = default)
        where TRequestFrom : IAPIRequest
        where TRequestTo : ICQRSRequest
    {
        var requestTo = mapper.Map<TRequestFrom, TRequestTo>(requestFrom);
        return await mediator.Send(requestTo, cancellationToken);
    }

    public async Task<ICQRSResponse> Send<TRequestFrom, TRequestTo>(TRequestFrom requestFrom, MutateRequest<TRequestTo> mutateRequest, CancellationToken cancellationToken = default)
        where TRequestFrom : IAPIRequest
        where TRequestTo : ICQRSRequest
    {
        var requestTo = mutateRequest(mapper.Map<TRequestFrom, TRequestTo>(requestFrom));
        return await mediator.Send(requestTo, cancellationToken);
    }

    public async Task<IActionResult> SendThenMap<TRequestFrom, TRequestTo>(TRequestFrom requestFrom, CancellationToken cancellationToken = default)
        where TRequestFrom : IAPIRequest
        where TRequestTo : ICQRSRequest
    {
        var response = await Send<TRequestFrom, TRequestTo>(requestFrom, cancellationToken);
        return responseMapper.Map(response);
    }

    public async Task<IActionResult> SendThenMap<TRequestFrom, TRequestTo>(TRequestFrom requestFrom, MutateRequest<TRequestTo> mutateRequest, CancellationToken cancellationToken = default)
        where TRequestFrom : IAPIRequest
        where TRequestTo : ICQRSRequest
    {
        var response = await Send(requestFrom, mutateRequest, cancellationToken);
        return responseMapper.Map(response);
    }
}