namespace SystemBase;

public static class ResponseTypeMapRegistryExtensions
{
    public static IResponseTypeMapRegistry Register<TResponse, TAPIResponse>(this IResponseTypeMapRegistry registry, int statusCode) 
        where TResponse : ICQRSResponse
        where TAPIResponse : IAPIResponse
    {
        return registry.Register(typeof(TResponse), typeof(TAPIResponse), statusCode);
    }

    public static IResponseTypeMapRegistry RegisterOK<TResponse, TAPIResponse>(this IResponseTypeMapRegistry registry) 
        where TResponse : ICQRSResponse
        where TAPIResponse : IAPIResponse
    {
        return registry.Register<TResponse, TAPIResponse>(StatusCodes.Status200OK);
    }

    public static IResponseTypeMapRegistry RegisterCreated<TResponse, TAPIResponse>(this IResponseTypeMapRegistry registry) 
        where TResponse : ICQRSResponse
        where TAPIResponse : IAPIResponse
    {
        return registry.Register<TResponse, TAPIResponse>(StatusCodes.Status201Created);
    }

    public static IResponseTypeMapRegistry RegisterBadRequest<TResponse, TAPIResponse>(this IResponseTypeMapRegistry registry) 
        where TResponse : ICQRSResponse
        where TAPIResponse : IAPIResponse
    {
        return registry.Register<TResponse, TAPIResponse>(StatusCodes.Status400BadRequest);
    }

    public static IResponseTypeMapRegistry RegisterNotFound<TResponse, TAPIResponse>(this IResponseTypeMapRegistry registry) 
        where TResponse : ICQRSResponse
        where TAPIResponse : IAPIResponse
    {
        return registry.Register<TResponse, TAPIResponse>(StatusCodes.Status404NotFound);
    }

    public static IResponseTypeMapRegistry RegisterUnauthorized<TResponse, TAPIResponse>(this IResponseTypeMapRegistry registry) 
        where TResponse : ICQRSResponse
        where TAPIResponse : IAPIResponse
    {
        return registry.Register<TResponse, TAPIResponse>(StatusCodes.Status401Unauthorized);
    }
}