namespace SystemBase;

sealed class UserIdAuditInfoSource : IAuditInfoSource
{
    readonly IHttpContextAccessor _contextAccessor;
    readonly string _claimType;

    public UserIdAuditInfoSource(IHttpContextAccessor contextAccessor, string claimType)
    {
        _contextAccessor = contextAccessor;
        _claimType = claimType;
    }

    public ValueTask Load(AuditInfo info, CancellationToken cancellationToken = default)
    {
        info.Set(AuditInfo.Keys.UserId, () =>
        {
            if (_contextAccessor.HttpContext is not HttpContext context || context.User.Identity is not IIdentity identity || !identity.IsAuthenticated || !context.User.TryGetClaimInt32(_claimType, out int value))
            {
                return null;
            }

            return value;
        });

        return ValueTask.CompletedTask;
    }
}
