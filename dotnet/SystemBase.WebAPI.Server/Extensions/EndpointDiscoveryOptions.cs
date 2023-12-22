namespace SystemBase;

public static class EndpointDiscoveryOptionsExtensions
{
    public static EndpointDiscoveryOptions WithDisabledAutoDiscovery(this EndpointDiscoveryOptions options, bool value = true)
    {
        options.DisableAutoDiscovery = value;
        return options;
    }

    public static EndpointDiscoveryOptions WithAssemblies(this EndpointDiscoveryOptions options, System.Reflection.Assembly[] assemblies)
    {
        options.Assemblies = assemblies;
        return options;
    }
}