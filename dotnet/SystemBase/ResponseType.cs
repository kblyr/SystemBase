namespace SystemBase;

[AttributeUsage(AttributeTargets.Class)]
public class ResponseTypeAttribute : Attribute
{
    public ResponseTypeAttribute() {}

    public string Type { get; init; } = "";
    public int StatusCode { get; init; }
    public bool IsEmpty { get; init; }
}

public class ResponseTypeAttribute<T> : ResponseTypeAttribute where T : ResponseType, new() {}

public record ResponseType
{
    string? _name;
    public string Name 
    {
        get => _name ?? StringUtilities.ToSnakeCase(GetType().Name);
        set => _name = value;
    }
    
    public ResponseStatusCode StatusCode { get; set; } = ResponseStatusCode.Default();
}

public record ResponseType<TStatusCode> : ResponseType where TStatusCode : ResponseStatusCode, new()
{
    public ResponseType() => StatusCode = ResponseStatusCode.GetCached<TStatusCode>();
}

public static class ResponseTypes
{
    public record OK : ResponseType<ResponseStatusCodes.OK>
    {
        public OK() => Name = "ok";
    }

    public record Created : ResponseType<ResponseStatusCodes.Created>
    {
        public Created() => Name = "created";
    }

    public record BadRequest : ResponseType<ResponseStatusCodes.BadRequest>
    {
        public BadRequest() => Name = "bad_request";
    }

    public record Unauthorized : ResponseType<ResponseStatusCodes.Unauthorized>
    {
        public Unauthorized() => Name = "unauthorized";
    }

    public record Forbidden : ResponseType<ResponseStatusCodes.Forbidden>
    {
        public Forbidden() => Name = "forbidden";
    }

    public record NotFound : ResponseType<ResponseStatusCodes.NotFound> 
    {
        public NotFound() => Name = "not_found";
    }

    public record PermissionDenied : Forbidden
    {
        public PermissionDenied() => Name = "permission_denied";
    }

    public static class PasswordHashManager
    {
        public record Computed : OK
        {
            public Computed() => Name = "password_hash_manager:computed";
        }

        public record IncorrectPassword : BadRequest
        {
            public IncorrectPassword() => Name = "password_hash_manager:incorrect_password";
        }

        public record InvalidHashedPassword : BadRequest
        {
            public InvalidHashedPassword() => Name = "password_hash_manager:invalid_hashed_password";
        }

        public record InvalidInputPassword : BadRequest
        {
            public InvalidInputPassword() => Name = "password_hash_manager:invalid_input_password";
        }

        public record InvalidSalt : BadRequest
        {
            public InvalidSalt() => Name = "password_hash_manager:invalid_salt";
        }

        public record Verified : OK
        {
            public Verified() => Name = "password_hash_manager:verified";
        }
    }
}