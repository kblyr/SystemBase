namespace SystemBase;

sealed class UserIdAuditInfoSource(IHttpContextAccessor contextAccessor, string claimType) : IAuditInfoSource
{
    public ValueTask Load(AuditInfo info, CancellationToken cancellationToken = default)
    {
        info.Set(AuditInfo.Keys.UserId, () =>
        {
            if (contextAccessor.HttpContext is not HttpContext context || context.User.Identity is not IIdentity identity || !identity.IsAuthenticated || !context.User.TryGetClaimInt32(claimType, out int value))
            {
                return null;
            }

            return value;
        });

        return ValueTask.CompletedTask;
    }
}
