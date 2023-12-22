namespace SystemBase.WebAPI;

public sealed record DependencyOptions : IDependencyOptions
{
    internal DependencyOptions() {}

    public DependencyFeature APIResponseTypeKeyProvider { get; } = DependencyFeature.Default;
}

public static class DependencyExtensions
{
    public static IServiceCollection AddSystemBaseWebAPI(this IServiceCollection services, ConfigureOptions<DependencyOptions>? configure = null)
    {
        var options = new DependencyOptions();
        configure?.Invoke(options);
        services.TryAddSingleton<IAPIResponseTypeRegistryKeyProvider, APIResponseTypeRegistryKeyProvider>(options.APIResponseTypeKeyProvider);
        return services;
    }
}