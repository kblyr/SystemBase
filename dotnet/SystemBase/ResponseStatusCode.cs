namespace SystemBase;

public record ResponseStatusCode
{
    public int Code { get; set; } = 200;

    static readonly KeyedCachedData<Type, ResponseStatusCode> _cached = new();

    public static ResponseStatusCode GetCached<T>() where T : ResponseStatusCode, new()
    {
        var type = typeof(T);
        if (_cached.TryGet(type, out var code))
        {
            return code;
        }

        return _cached.Set(type, new T());
    }

    public static ResponseStatusCode Default() => GetCached<ResponseStatusCodes.OK>();
}

public static class ResponseStatusCodes
{
    public record OK : ResponseStatusCode
    {
        public OK() => Code = 200;
    }

    public record Created : ResponseStatusCode
    {
        public Created() => Code = 201;
    }

    public record BadRequest : ResponseStatusCode
    {
        public BadRequest() => Code = 400;
    }

    public record Unauthorized : ResponseStatusCode
    {
        public Unauthorized() => Code = 401;
    }

    public record Forbidden : ResponseStatusCode
    {
        public Forbidden() => Code = 403;
    }

    public record NotFound : ResponseStatusCode
    {
        public NotFound() => Code = 404;
    }
}