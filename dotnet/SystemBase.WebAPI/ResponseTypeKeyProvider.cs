namespace SystemBase;

public interface IAPIResponseTypeRegistryKeyProvider
{
    string Get(Type responseType);
}

sealed class APIResponseTypeRegistryKeyProvider : IAPIResponseTypeRegistryKeyProvider
{
    static readonly Type t_schemaIdAttr = typeof(SchemaIdAttribute);

    readonly Dictionary<Type, string> _registryKeys = new();

    public string Get(Type responseType)
    {
        if (_registryKeys.ContainsKey(responseType))
        {
            return _registryKeys[responseType];
        }

        var schemaIdAttr = responseType.GetCustomAttributes(t_schemaIdAttr, false).FirstOrDefault() as SchemaIdAttribute;
        string? key = schemaIdAttr?.SchemaId ?? responseType.FullName;
        _registryKeys.Add(responseType, key ?? throw new FailedToGetAPIResponseTypeKeyException(responseType));
        return key;

    }
}