namespace SystemBase;

sealed class CurrentAuditInfoProvider : ICurrentAuditInfoProvider
{
    readonly IHttpContextAccessor _contextAccessor;
    readonly IHashIdConverter _hashIdConverter;
    readonly IAuditInfoExtractor _extractor;

    public CurrentAuditInfoProvider(IHttpContextAccessor contextAccessor, IHashIdConverter hashIdConverter, IAuditInfoExtractor extractor)
    {
        _contextAccessor = contextAccessor;
        _hashIdConverter = hashIdConverter;
        _extractor = extractor;
    }

    AuditInfo? _value;
    public AuditInfo Value => (_value ??= Get()) with { Timestamp = DateTimeOffset.Now };

    AuditInfo Get()
    {
        if (_contextAccessor.HttpContext is not null && _contextAccessor.HttpContext.User.Identity is IIdentity identity && identity.IsAuthenticated)
        {
            return _extractor.Extract(_contextAccessor.HttpContext.User);
        }

        return new AuditInfo();
    }
}