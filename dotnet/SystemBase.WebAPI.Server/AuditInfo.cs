namespace SystemBase;

sealed class CurrentAuditInfoProvider : ICurrentAuditInfoProvider
{
    readonly IHttpContextAccessor _contextAccessor;
    readonly IHashIdConverter _hashIdConverter;

    public CurrentAuditInfoProvider(IHttpContextAccessor contextAccessor, IHashIdConverter hashIdConverter)
    {
        _contextAccessor = contextAccessor;
        _hashIdConverter = hashIdConverter;
    }

    AuditInfo? _value;
    public AuditInfo Value => (_value ??= Get()) with { Timestamp = DateTimeOffset.Now };

    AuditInfo Get()
    {
        if (_contextAccessor.HttpContext is not null && _contextAccessor.HttpContext.User.Identity is IIdentity identity && identity.IsAuthenticated)
        {
            return new AuditInfo
            {
                UserId = _contextAccessor.HttpContext.User.TryGetClaim(ClaimTypes.User.Id, out string idString) ? _hashIdConverter.ToInt32(idString) : 0
            };
        }

        return new AuditInfo();
    }
}