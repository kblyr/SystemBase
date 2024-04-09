namespace SystemBase;

public interface IResponseMapper
{
    IActionResult Map(ICQRSResponse response);
}

sealed class ResponseMapper(IResponseTypeMapRegistry registry, IAPIResponseTypeRegistryKeyProvider registryKeyProvider, MapsterMapper.IMapper mapper, IHttpContextAccessor contextAccessor) : IResponseMapper
{
    readonly IResponseTypeMapRegistry _registry = registry;
    readonly IAPIResponseTypeRegistryKeyProvider _registryKeyProvider = registryKeyProvider;
    readonly MapsterMapper.IMapper _mapper = mapper;
    readonly IHttpContextAccessor _contextAccessor = contextAccessor;

    public IActionResult Map(ICQRSResponse response)
    {
        if (response is null || !_registry.TryGet(response.GetType(), out ResponseTypeMapDefinition definition))
        {
            return new ObjectResult(null)
            { 
                StatusCode = StatusCodes.Status204NoContent
            };
        }

        _contextAccessor.HttpContext?.Response.Headers.Append(APIHeaders.ResponseType, _registryKeyProvider.Get(definition.ApiResponseType));
        return new ObjectResult(_mapper.Map(response, definition.ResponseType, definition.ApiResponseType))
        {
            StatusCode = definition.StatusCode
        };
    }
}