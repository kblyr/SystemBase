namespace SystemBase;

public static class ServiceCollectionExtensions
{
    public static void TryAddSingleton<TService, TImplementation>(this IServiceCollection services, bool condition) 
        where TService : class
        where TImplementation : class, TService
    {
        if (!condition)
        {
            return;
        }

        services.TryAddSingleton<TService, TImplementation>();
    }

    public static void TryAddSingleton<TService, TImplementation>(this IServiceCollection services, bool condition, Action<IServiceCollection> with) 
        where TService : class
        where TImplementation : class, TService
    {
        if (!condition)
        {
            return;
        }

        services.TryAddSingleton<TService, TImplementation>();
        with(services);
    }

    public static void TryAddScoped<TService, TImplementation>(this IServiceCollection services, bool condition) 
        where TService : class
        where TImplementation : class, TService
    {
        if (!condition)
        {
            return;
        }

        services.TryAddScoped<TService, TImplementation>();
    }

    public static void TryAddScoped<TService, TImplementation>(this IServiceCollection services, bool condition, Action<IServiceCollection> with) 
        where TService : class
        where TImplementation : class, TService
    {
        if (!condition)
        {
            return;
        }

        services.TryAddScoped<TService, TImplementation>();
        with(services);
    }

    public static void TryAddTransient<TService, TImplementation>(this IServiceCollection services, bool condition) 
        where TService : class
        where TImplementation : class, TService
    {
        if (!condition)
        {
            return;
        }

        services.TryAddTransient<TService, TImplementation>();
    }

    public static void TryAddTransient<TService, TImplementation>(this IServiceCollection services, bool condition, Action<IServiceCollection> with) 
        where TService : class
        where TImplementation : class, TService
    {
        if (!condition)
        {
            return;
        }

        services.TryAddTransient<TService, TImplementation>();
        with(services);
    }
}