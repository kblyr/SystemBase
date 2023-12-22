namespace SystemBase;

public class APIEndpoint<TAPIRequest> : Endpoint<TAPIRequest> where TAPIRequest : IAPIRequest, new()
{
    IAPIMediator? _mediator;
    protected IAPIMediator Mediator => _mediator ??= Resolve<IAPIMediator>();

    protected async Task Send<TRequest>(TAPIRequest request, CancellationToken cancellationToken = default) where TRequest : ICQRSRequest
    {
        var response = await Mediator.Send<TAPIRequest, TRequest>(request, cancellationToken);
        await SendApiResponse(response, cancellationToken);
    }

    protected async Task Send<TRequest>(TAPIRequest request, MutateRequest<TRequest> mutateRequest, CancellationToken cancellationToken = default) where TRequest : ICQRSRequest
    {
        var response = await Mediator.Send(request, mutateRequest, cancellationToken);
        await SendApiResponse(response, cancellationToken);
    }

    async Task SendApiResponse(ICQRSResponse response, CancellationToken cancellationToken)
    {
        var registry = Resolve<IResponseTypeMapRegistry>();
        var registryKeyProvider = Resolve<IAPIResponseTypeRegistryKeyProvider>();
        var mapper = Resolve<MapsterMapper.IMapper>();

        if (response is null || registry.TryGet(response.GetType(), out ResponseTypeMapDefinition definition) == false)
        {
            await SendOkAsync(response ?? new object(), cancellationToken);
            return;
        }

        HttpContext.Response.Headers.Add(APIHeaders.ResponseType, registryKeyProvider.Get(definition.ApiResponseType));
        await SendAsync(mapper.Map(response, definition.ResponseType, definition.ApiResponseType), definition.StatusCode, cancellationToken);
    }

    protected void ResponseCache(TimeSpan duration)
    {
        ResponseCache(Convert.ToInt32(duration.TotalSeconds));
    }
}

public class APIEndpoint<TAPIRequest, TRequest> : APIEndpoint<TAPIRequest>
    where TAPIRequest : IAPIRequest, new()
    where TRequest : ICQRSRequest
{
    public override async Task HandleAsync(TAPIRequest req, CancellationToken ct)
    {
        await Send<TRequest>(req, MutateRequest, ct);
    }

    protected virtual TRequest MutateRequest(TRequest request) => request;
}