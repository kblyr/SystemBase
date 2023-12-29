namespace SystemBase;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDbContextFactory<TContext>(this IServiceCollection services, DependencyFeature<ContextSettings> feature) where TContext : DbContext
    {
        if (feature)
        {
            services.AddDbContextFactory<TContext>(feature.Settings.ConfigureOptionsBuilder);
        }
        
        return services;
    }
}