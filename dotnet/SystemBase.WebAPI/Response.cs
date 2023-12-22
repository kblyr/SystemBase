namespace SystemBase;

public interface IAPIResponse {}

public interface IAPIErrorResponse : IAPIResponse {}

[SchemaId(SchemaIds.Success)]
public sealed record SuccessAPI : IAPIResponse {}

[SchemaId(SchemaIds.Failed)]
public sealed record FailedAPI : IAPIErrorResponse {}

[SchemaId(SchemaIds.VerifyPermissionFailed)]
public sealed record VerifyPermissionFailedAPI : IAPIErrorResponse
{
    public required string PermissionId { get; init; }
}