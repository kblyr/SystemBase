namespace SystemBase;

public interface IPermissionVerifier
{
    ValueTask<bool> IsAdministrator(CancellationToken cancellationToken = default);
    ValueTask<bool> Verify(int permissionId, CancellationToken cancellationToken = default);
}

sealed class PermissionVerifier(IPermissionsLoader permissionsLoader) : IPermissionVerifier
{   
    PermissionsLoaderResult? _loaderResult;

    public async ValueTask<bool> IsAdministrator(CancellationToken cancellationToken = default)
    {
        return (await TryLoad(cancellationToken)).IsAdministrator;
    }

    public async ValueTask<bool> Verify(int permissionId, CancellationToken cancellationToken = default)
    {
        if (permissionId == 0)
        {
            return false;
        }

        var result = await TryLoad(cancellationToken);
        return result.IsAdministrator || (result.PermissionIds.Length > 0 && result.PermissionIds.Contains(permissionId));
    }

    async ValueTask<PermissionsLoaderResult> TryLoad(CancellationToken cancellationToken)
    {
        return _loaderResult ??= await permissionsLoader.Load(cancellationToken) ?? new();
    }
}