namespace SystemBase;

[SchemaId(SchemaIds.VerifyPermissionFailed)]
public sealed record VerifyPermissionFailedAPI : IAPIErrorResponse
{
    public required string PermissionId { get; init; }
}