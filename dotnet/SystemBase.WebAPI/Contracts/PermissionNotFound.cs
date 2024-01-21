namespace SystemBase;

[SchemaId(SchemaIds.PermissionNotFound)]
public sealed record PermissionNotFoundAPI : IAPIErrorResponse
{
    public required string Id { get; init; }
}