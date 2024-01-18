using System.Collections.Concurrent;

namespace SystemBase;

public sealed class AuditInfo
{
    readonly ConcurrentDictionary<string, IAuditInfoValue> _source = new();

    internal AuditInfo() {}

    public T Get<T>(string key)
    {
        if (_source.TryGetValue(key, out IAuditInfoValue? value) && value.Value is T genericValue)
        {
            return genericValue;
        }

        return default!;
    }

    void Set(string key, IAuditInfoValue value)
    {
        _source.AddOrUpdate(key, value, (existingKey, existingValue) => value);
    }

    public void Set(string key, object? value)
    {
        var infoValue = new AuditInfoValueReadOnly(value);
        Set(key, infoValue);
    }

    public void Set(string key, Func<object?> factory)
    {
        var infoValue = new AuditInfoValueFactory(factory);
        Set(key, infoValue);
    }

    public void Set(string key, Func<AuditInfo, object?> factory)
    {
        var infoValue = new AuditInfoValueDependentFactory(this, factory);
        Set(key, infoValue);
    }

    public static class Keys
    {
        public const string Timestamp   = "SystemBase:Timestamp";
        public const string UserId      = "SystemBase:UserId";
    }
}

public interface IAuditInfoValue
{
    public object? Value { get; }
}

sealed class AuditInfoValueReadOnly : IAuditInfoValue
{
    public object? Value { get; }

    internal AuditInfoValueReadOnly(object? value) 
    {
        Value = value;
    }
}

sealed class AuditInfoValueFactory : IAuditInfoValue
{
    readonly Func<object?> _factory;
    public object? Value => _factory();

    public AuditInfoValueFactory(Func<object?> factory)
    {
        _factory = factory;
    }
}

sealed class AuditInfoValueDependentFactory : IAuditInfoValue
{
    readonly AuditInfo _info;
    readonly Func<AuditInfo, object?> _factory;
    public object? Value => _factory(_info);

    public AuditInfoValueDependentFactory(AuditInfo info, Func<AuditInfo, object?> factory)
    {
        _info = info;
        _factory = factory;
    }
}

public interface IAuditInfoSource
{
    ValueTask Load(AuditInfo info, CancellationToken cancellationToken = default);
}

public interface IAuditInfoProvider 
{
    ValueTask<AuditInfo> Get(CancellationToken cancellationToken = default);
}

sealed class AuditInfoProvider : IAuditInfoProvider
{
    readonly IEnumerable<IAuditInfoSource> _sources;

    public AuditInfoProvider(IEnumerable<IAuditInfoSource> sources)
    {
        _sources = sources;
    }

    public async ValueTask<AuditInfo> Get(CancellationToken cancellationToken = default)
    {
        var auditInfo = new AuditInfo();

        foreach(var source in _sources)
        {
            await source.Load(auditInfo, cancellationToken);
        }

        return auditInfo;
    }
}

sealed class TimestampAuditInfoSource : IAuditInfoSource
{
    public ValueTask Load(AuditInfo info, CancellationToken cancellationToken = default)
    {
        info.Set(AuditInfo.Keys.Timestamp, () => DateTimeOffset.Now);
        return ValueTask.CompletedTask;
    }
}