namespace SystemBase;

public interface IResponseMapper
{
    IActionResult Map(ICQRSResponse response);
}

sealed class ResponseMapper(IResponseTypeMapRegistry registry, IAPIResponseTypeRegistryKeyProvider registryKeyProvider, MapsterMapper.IMapper mapper, IHttpContextAccessor contextAccessor) : IResponseMapper
{
    public IActionResult Map(ICQRSResponse response)
    {
        if (response is null || !registry.TryGet(response.GetType(), out ResponseTypeMapDefinition definition))
        {
            return new ObjectResult(null)
            { 
                StatusCode = StatusCodes.Status204NoContent
            };
        }

        contextAccessor.HttpContext?.Response.Headers.Append(APIHeaders.ResponseType, registryKeyProvider.Get(definition.ApiResponseType));
        return new ObjectResult(mapper.Map(response, definition.ResponseType, definition.ApiResponseType))
        {
            StatusCode = definition.StatusCode
        };
    }
}