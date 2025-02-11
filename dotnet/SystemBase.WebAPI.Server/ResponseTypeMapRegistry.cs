using System.Collections.Concurrent;
using System.Reflection;

namespace SystemBase;

public interface IResponseTypeMapRegistry
{
    ResponseTypeMapDefinition? this[Type responseType] { get; }
    bool TryGet(Type responseType, out ResponseTypeMapDefinition definition);
    IResponseTypeMapRegistry Register(ResponseTypeMapDefinition definition);
    IResponseTypeMapRegistry Register(Type responseType, Type apiResponseType, int statusCode);
}

public interface IResponseTypeMapRegistration
{
    void Register(IResponseTypeMapRegistry registry);
}

public record struct ResponseTypeMapDefinition
(
    Type ResponseType,
    Type ApiResponseType,
    int StatusCode
);

sealed class ResponseTypeMapRegistry(ResponseTypeMapAssemblyScanner assemblyScanner) : IResponseTypeMapRegistry
{
    readonly ConcurrentDictionary<Type, ResponseTypeMapDefinition> _definitions = [];

    public ResponseTypeMapDefinition? this[Type responseType]
    {
        get
        {
            if (_definitions.TryGetValue(responseType, out ResponseTypeMapDefinition value))
            {
                return value;
            }

            if (!assemblyScanner.IsScanned)
            {
                assemblyScanner.Scan(this);
                return this[responseType];
            }

            return null;
        }
    }

    public IResponseTypeMapRegistry Register(ResponseTypeMapDefinition definition)
    {
        Register(definition.ResponseType, definition.ApiResponseType, definition.StatusCode);
        return this;
    }

    public IResponseTypeMapRegistry Register(Type responseType, Type apiResponseType, int statusCode)
    {
        var value = new ResponseTypeMapDefinition(responseType, apiResponseType, statusCode);
        _definitions.AddOrUpdate(responseType, value, (existingKey, existingValue) => value);
        return this;
    }

    public bool TryGet(Type responseType, out ResponseTypeMapDefinition definition)
    {
        if (_definitions.TryGetValue(responseType, out ResponseTypeMapDefinition value))
        {
            definition = value;
            return true;
        }

        if (!assemblyScanner.IsScanned)
        {
            assemblyScanner.Scan(this);
            return TryGet(responseType, out definition);
        }

        definition = default;
        return false;
    }
}

public sealed class ResponseTypeMapAssemblyScanner(Assembly[] assemblies)
{
    public bool IsScanned { get; private set; }

    public void Scan(IResponseTypeMapRegistry registry)
    {
        IsScanned = true;

        if (assemblies.Length == 0)
        {
            return;
        }

        var t_marker = typeof(IResponseTypeMapRegistration);

        for (int assemblyIdx = 0; assemblyIdx < assemblies.Length; assemblyIdx++)
        {
            var t_registrations = assemblies[assemblyIdx].GetTypes().Where(t =>
                !t.IsAbstract
                && !t.IsGenericType
                && t.GetInterfaces().Any(t_interface => t_interface == t_marker)
                && t.GetConstructor(Type.EmptyTypes) is not null
            ).ToArray();

            if (t_registrations.Length == 0)
            {
                continue;
            }

            for (var registrationIdx = 0; registrationIdx < t_registrations.Length; registrationIdx++)
            {
                var registration = Activator.CreateInstance(t_registrations[registrationIdx]) as IResponseTypeMapRegistration;
                registration?.Register(registry);
            }
        }
    }
}