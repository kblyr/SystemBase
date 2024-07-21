namespace SystemBase;

public interface IPermissionsLoader
{
    Task<PermissionsLoaderResult> Load(CancellationToken cancellationToken = default);
}

public sealed record PermissionsLoaderResult
{
    public bool IsAdministrator { get; init; }
    public int[] PermissionIds { get; init; } = [];
}