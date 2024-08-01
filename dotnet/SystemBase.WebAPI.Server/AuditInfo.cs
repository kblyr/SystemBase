namespace SystemBase;

sealed class UserIdAuditInfoSource(IHttpContextAccessor contextAccessor, Func<ClaimsPrincipal, object?> getFromClaims) : IAuditInfoSource
{
    public ValueTask Load(AuditInfo info, CancellationToken cancellationToken = default)
    {
        info.Set(AuditInfo.Keys.UserId, () =>
        {
            if (contextAccessor.HttpContext is not HttpContext context || context.User.Identity is not IIdentity identity || !identity.IsAuthenticated)
            {
                return null;
            }

            return getFromClaims(context.User);
        });

        return ValueTask.CompletedTask;
    }
}
