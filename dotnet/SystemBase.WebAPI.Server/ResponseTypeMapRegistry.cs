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
    readonly Dictionary<Type, ResponseTypeMapDefinition> _definitions = [];
    readonly ResponseTypeMapAssemblyScanner _assemblyScanner = assemblyScanner;

    public ResponseTypeMapDefinition? this[Type responseType]
    {
        get
        {
            if (_definitions.ContainsKey(responseType))
            {
                return _definitions[responseType];
            }

            if (!_assemblyScanner.IsScanned)
            {
                _assemblyScanner.Scan(this);
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
        if (!_definitions.ContainsKey(responseType))
        {
            _definitions.Add(responseType, new(responseType, apiResponseType, statusCode));
        }

        return this;
    }

    public bool TryGet(Type responseType, out ResponseTypeMapDefinition definition)
    {
        if (_definitions.TryGetValue(responseType, out ResponseTypeMapDefinition value))
        {
            definition = value;
            return true;
        }

        if (!_assemblyScanner.IsScanned)
        {
            _assemblyScanner.Scan(this);
            return TryGet(responseType, out definition);
        }

        definition = default;
        return false;
    }
}

public sealed class ResponseTypeMapAssemblyScanner(Assembly[] assemblies)
{
    readonly Assembly[] _assemblies = assemblies;

    public bool IsScanned { get; private set; }

    public void Scan(IResponseTypeMapRegistry registry)
    {
        IsScanned = true;

        if (_assemblies.Length == 0)
        {
            return;
        }

        var t_marker = typeof(IResponseTypeMapRegistration);

        for (int assemblyIdx = 0; assemblyIdx < _assemblies.Length; assemblyIdx++)
        {
            var t_registrations = _assemblies[assemblyIdx].GetTypes().Where(t =>
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