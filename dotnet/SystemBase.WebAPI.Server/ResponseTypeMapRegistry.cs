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

sealed class ResponseTypeMapRegistry : IResponseTypeMapRegistry
{
    readonly Dictionary<Type, ResponseTypeMapDefinition> _definitions = new();
    readonly ResponseTypeMapAssemblyScanner _assemblyScanner;

    public ResponseTypeMapRegistry(ResponseTypeMapAssemblyScanner assemblyScanner)
    {
        _assemblyScanner = assemblyScanner;
    }

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
        if (!_definitions.ContainsKey(definition.ResponseType))
        {
            _definitions.Add(definition.ResponseType, definition);
        }

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
        if (_definitions.ContainsKey(responseType))
        {
            definition = _definitions[responseType];
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

public sealed class ResponseTypeMapAssemblyScanner 
{
    readonly Assembly[] _assemblies;

    public bool IsScanned { get; private set; }

    public ResponseTypeMapAssemblyScanner(Assembly[] assemblies)
    {
        _assemblies = assemblies;
    }

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