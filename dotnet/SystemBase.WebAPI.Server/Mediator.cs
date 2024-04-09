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

sealed class APIMediator : IAPIMediator
{
    readonly MediatR.IMediator _mediator;
    readonly MapsterMapper.IMapper _mapper;
    readonly IResponseMapper _responseMapper;

    public APIMediator(MediatR.IMediator mediator, MapsterMapper.IMapper mapper, IResponseMapper responseMapper)
    {
        _mediator = mediator;
        _mapper = mapper;
        _responseMapper = responseMapper;
    }

    public async Task<ICQRSResponse> Send<TRequestFrom, TRequestTo>(TRequestFrom requestFrom, CancellationToken cancellationToken = default)
        where TRequestFrom : IAPIRequest
        where TRequestTo : ICQRSRequest
    {
        var requestTo = _mapper.Map<TRequestFrom, TRequestTo>(requestFrom);
        return await _mediator.Send(requestTo, cancellationToken);
    }

    public async Task<ICQRSResponse> Send<TRequestFrom, TRequestTo>(TRequestFrom requestFrom, MutateRequest<TRequestTo> mutateRequest, CancellationToken cancellationToken = default)
        where TRequestFrom : IAPIRequest
        where TRequestTo : ICQRSRequest
    {
        var requestTo = mutateRequest(_mapper.Map<TRequestFrom, TRequestTo>(requestFrom));
        return await _mediator.Send(requestTo, cancellationToken);
    }

    public async Task<IActionResult> SendThenMap<TRequestFrom, TRequestTo>(TRequestFrom requestFrom, CancellationToken cancellationToken = default)
        where TRequestFrom : IAPIRequest
        where TRequestTo : ICQRSRequest
    {
        var response = await Send<TRequestFrom, TRequestTo>(requestFrom, cancellationToken);
        return _responseMapper.Map(response);
    }

    public async Task<IActionResult> SendThenMap<TRequestFrom, TRequestTo>(TRequestFrom requestFrom, MutateRequest<TRequestTo> mutateRequest, CancellationToken cancellationToken = default)
        where TRequestFrom : IAPIRequest
        where TRequestTo : ICQRSRequest
    {
        var response = await Send(requestFrom, mutateRequest, cancellationToken);
        return _responseMapper.Map(response);
    }
}