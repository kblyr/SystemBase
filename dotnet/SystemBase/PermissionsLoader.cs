namespace SystemBase;

public interface IPermissionsLoader
{
    ValueTask<PermissionsLoaded> Load(CancellationToken cancellationToken = default);
}

public sealed record PermissionsLoaded(bool IsAdministrator, string[] Permissions) : ISuccessResponse
{
    public static readonly PermissionsLoaded Empty = new(false, []);
}