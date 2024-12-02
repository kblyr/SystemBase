using System.Collections.Concurrent;

namespace SystemBase;

public class SessionData
{
    readonly ConcurrentDictionary<string, ISessionValue> _store = new();

    internal SessionData() {}

    CachedSessionData? _cached;
    public CachedSessionData Cached => _cached ??= new(this);

    public virtual T Get<T>(string key)
    {
        if (_store.TryGetValue(key, out ISessionValue? value) && value.Value is T genericValue)
        {
            return genericValue;
        }

        return default!;
    }

    void Set(string key, ISessionValue value)
    {
        _store.AddOrUpdate(key, value, (existingKey, existingValue) => value);
    }

    public void Set(string key, object? value)
    {
        Set(key, new SessionValueReadOnly(value));
    }

    public void Set(string key, Func<object?> factory)
    {
        Set(key, new SessionValueFactory(factory));
    }

    public static class Keys
    {
        public const string UserId = "UserId";
        public const string Timestamp = "Timestamp";
    }
}

public class CachedSessionData(SessionData source)
{
    readonly ConcurrentDictionary<string, object?> _cached = new();

    public virtual T Get<T>(string key)
    {
        if (_cached.TryGetValue(key, out object? cachedValue) && cachedValue is T genericValue)
        {
            return genericValue;
        }

        var value = source.Get<T>(key);
        _cached.AddOrUpdate(key, value, (existingKey, existingValue) => value);
        return value;
    }
}

public interface ISessionValue
{
    object? Value { get; }
}

sealed record SessionValueReadOnly(object? Value) : ISessionValue;

sealed record SessionValueFactory(Func<object?> Factory) : ISessionValue
{
    public object? Value => Factory();
}

public interface ISessionDataSource
{
    ValueTask Load(SessionData data, CancellationToken cancellationToken = default);
}

public interface ISessionDataProvider
{
    ValueTask<SessionData> Get(CancellationToken cancellationToken = default);
    ValueTask<CachedSessionData> GetCached(CancellationToken cancellationToken = default);
}

sealed class SessionDataProvider(IEnumerable<ISessionDataSource> sources) : ISessionDataProvider
{
    SessionData? _data;
    SessionData Data => _data ??= new();

    public async ValueTask<SessionData> Get(CancellationToken cancellationToken = default)
    {
        foreach (var source in sources)
        {
            await source.Load(Data, cancellationToken);
        }

        return Data;
    }

    public async ValueTask<CachedSessionData> GetCached(CancellationToken cancellationToken = default)
    {
        return (await Get(cancellationToken)).Cached;
    }
}

sealed class TimestampSessionDataSource(Func<DateTimeOffset> factory) : ISessionDataSource
{
    public ValueTask Load(SessionData data, CancellationToken cancellationToken = default)
    {
        data.Set(SessionData.Keys.Timestamp, () => factory());
        return ValueTask.CompletedTask;
    }
}
