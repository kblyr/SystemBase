using System.Reflection;

namespace SystemBase.WebAPI.Server;

public sealed record DependencyOptions : IDependencyOptions
{
    internal DependencyOptions() {}

    public DependencyFeature AccessTokenGenerator { get; } = DependencyFeature.Default();
    public DependencyFeature APIMediator { get; } = DependencyFeature.Default();
    public DependencyFeature ResponseMapper { get; } = DependencyFeature.Default();
    public DependencyFeature<ResponseTypeMapRegistrySettings> ResponseTypeMapRegistry { get; } = DependencyFeature<ResponseTypeMapRegistrySettings>.Default();
}

public sealed record ResponseTypeMapRegistrySettings : IDependencyFeatureSettings
{
    Assembly[] _assemblies = [];
    public Assembly[] Assemblies
    {
        get => _assemblies;
        set => _assemblies = value;
    }
}

public static class DependencyExtensions
{
    public static IServiceCollection AddSystemBaseWebAPIServer(this IServiceCollection services, ConfigureOptions<DependencyOptions>? configure = null)
    {
        var options = new DependencyOptions();
        configure?.Invoke(options);
        services.TryAddTransient<IAPIMediator, APIMediator>(options.APIMediator);
        services.TryAddTransient<IResponseMapper, ResponseMapper>(options.ResponseMapper);
        services.TryAddSingleton<IAccessTokenGenerator, AccessTokenGenerator>(options.AccessTokenGenerator);
        services.TryAddSingleton<IResponseTypeMapRegistry, ResponseTypeMapRegistry>(options.ResponseTypeMapRegistry, services => services.TryAddSingleton(sp => new ResponseTypeMapAssemblyScanner(options.ResponseTypeMapRegistry.Settings.Assemblies)));
        return services;
    }
}

public sealed class AssemblyMarker : IAssemblyMarker {}