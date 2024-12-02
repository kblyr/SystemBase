namespace SystemBase;

public interface IPermissionVerifier
{
    ValueTask<bool> IsAdministrator(CancellationToken cancellationToken = default);
    ValueTask<IResponse> Verify(string permission, CancellationToken cancellationToken = default);
    ValueTask<IResponse> Verify<T>(CancellationToken cancellationToken = default) where T : PermissionId, new();
}

public sealed record PermissionVerified : ISuccessResponse
{
    static readonly PermissionVerified _instance = new();
    public static PermissionVerified Instance => _instance;
}

[ResponseType<ResponseTypes.PermissionDenied>]
public record PermissionDenied : IFailureResponse
{
    public string Permission { get; init; } = "";
}

sealed class PermissionVerifier(IPermissionsLoader permissionsLoader) : IPermissionVerifier
{
    public async ValueTask<bool> IsAdministrator(CancellationToken cancellationToken = default)
    {
        return (await permissionsLoader.Load(cancellationToken)).IsAdministrator;
    }

    public async ValueTask<IResponse> Verify(string permission, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(permission))
        {
            return new PermissionDenied { Permission = permission };
        }

        var permissions = await permissionsLoader.Load(cancellationToken);
        return (permissions.IsAdministrator || permissions.Permissions.Contains(permission))
            ? PermissionVerified.Instance
            : new PermissionDenied { Permission = permission };
    }

    public ValueTask<IResponse> Verify<T>(CancellationToken cancellationToken = default) where T : PermissionId, new()
    {
        return Verify(PermissionId.GetCached<T>().Id, cancellationToken);
    }
}