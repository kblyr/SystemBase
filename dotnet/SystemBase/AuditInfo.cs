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
        Set(key, new AuditInfoValueReadOnly(value));
    }

    public void Set(string key, Func<object?> factory)
    {
        Set(key, new AuditInfoValueFactory(factory));
    }

    public void Set(string key, Func<AuditInfo, object?> factory)
    {
        Set(key, new AuditInfoValueDependentFactory(this, factory));
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

sealed class AuditInfoValueReadOnly(object? value) : IAuditInfoValue
{
    public object? Value { get; } = value;
}

sealed class AuditInfoValueFactory(Func<object?> factory) : IAuditInfoValue
{
    readonly Func<object?> _factory = factory;
    public object? Value => _factory();
}

sealed class AuditInfoValueDependentFactory(AuditInfo info, Func<AuditInfo, object?> factory) : IAuditInfoValue
{
    readonly AuditInfo _info = info;
    readonly Func<AuditInfo, object?> _factory = factory;
    public object? Value => _factory(_info);
}

public interface IAuditInfoSource
{
    ValueTask Load(AuditInfo info, CancellationToken cancellationToken = default);
}

public interface IAuditInfoProvider 
{
    ValueTask<AuditInfo> Get(CancellationToken cancellationToken = default);
}

sealed class AuditInfoProvider(IEnumerable<IAuditInfoSource> sources) : IAuditInfoProvider
{
    readonly IEnumerable<IAuditInfoSource> _sources = sources;

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

sealed class TimestampAuditInfoSource(Func<DateTimeOffset> factory) : IAuditInfoSource
{
    readonly Func<DateTimeOffset> _factory = factory;

    public ValueTask Load(AuditInfo info, CancellationToken cancellationToken = default)
    {
        info.Set(AuditInfo.Keys.Timestamp, () => _factory());
        return ValueTask.CompletedTask;
    }
}