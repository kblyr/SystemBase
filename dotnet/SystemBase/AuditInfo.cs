using System.Collections.Concurrent;

namespace SystemBase;

public class AuditInfo
{
    readonly ConcurrentDictionary<string, IAuditInfoValue> _source = new();

    internal AuditInfo() {}

    CachedAuditInfo? _cached;
    public CachedAuditInfo Cached => _cached ??= new(this);

    public virtual T Get<T>(string key)
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
        public static string Timestamp { get; set; } = "SystemBase:Timestamp";
        public static string UserId { get; set; } = "SystemBase:UserId";
    }
}

public sealed class CachedAuditInfo(AuditInfo source) : AuditInfo
{
    readonly ConcurrentDictionary<string, object?> _cached = new();

    public override T Get<T>(string key)
    {
        if (_cached.TryGetValue(key, out object? cachedValue) && cachedValue is T genericCachedValue)
        {
            return genericCachedValue;
        }

        var value = source.Get<T>(key);
        _cached.AddOrUpdate(key, value, (existingKey, existingValue) => value);
        return value;
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
    public object? Value => factory();
}

sealed class AuditInfoValueDependentFactory(AuditInfo info, Func<AuditInfo, object?> factory) : IAuditInfoValue
{
    public object? Value => factory(info);
}

public interface IAuditInfoSource
{
    ValueTask Load(AuditInfo info, CancellationToken cancellationToken = default);
}

public interface IAuditInfoProvider 
{
    ValueTask<AuditInfo> Get(CancellationToken cancellationToken = default);
    ValueTask<CachedAuditInfo> GetCached(CancellationToken cancellationToken = default);
}

sealed class AuditInfoProvider(IEnumerable<IAuditInfoSource> sources) : IAuditInfoProvider
{
    AuditInfo? _auditInfo;
    AuditInfo AuditInfo => _auditInfo ??= new();

    public async ValueTask<AuditInfo> Get(CancellationToken cancellationToken = default)
    {
        foreach(var source in sources)
        {
            await source.Load(AuditInfo, cancellationToken);
        }

        return AuditInfo;
    }

    public async ValueTask<CachedAuditInfo> GetCached(CancellationToken cancellationToken = default)
    {
        return (await Get(cancellationToken)).Cached;
    }
}

sealed class TimestampAuditInfoSource(Func<DateTimeOffset> factory) : IAuditInfoSource
{
    public ValueTask Load(AuditInfo info, CancellationToken cancellationToken = default)
    {
        info.Set(AuditInfo.Keys.Timestamp, () => factory());
        return ValueTask.CompletedTask;
    }
}