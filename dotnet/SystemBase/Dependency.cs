using System.Reflection;

namespace SystemBase;

public interface IDependencyOptions {}

public record DependencyFeature
{
    public bool IsIncluded { get; private set; } = false;

    public DependencyFeature Include(bool isIncluded = true)
    {
        IsIncluded = isIncluded;
        return this;
    }

    public static readonly DependencyFeature Default = new();

    public static implicit operator bool(DependencyFeature feature) => feature.IsIncluded;
}

public record DependencyFeature<TSettings> : DependencyFeature where TSettings : class, IDependencyFeatureSettings, new()
{
    public TSettings Settings { get; } = new();

    public DependencyFeature<TSettings> Include(Action<TSettings> configureSettings)
    {
        configureSettings(Settings);
        Include(true);
        return this;
    }

    public static new readonly DependencyFeature<TSettings> Default = new();
}

public interface IDependencyFeatureSettings {}

public delegate void ConfigureOptions<T>(T options) where T : IDependencyOptions;

public interface IAssemblyMarker {}

public sealed class AssemblyMarker : IAssemblyMarker
{
    public static MarkerGroup Group()
    {
        return new();
    }

    public sealed class MarkerGroup
    {
        readonly HashSet<Assembly> _assemblies = new();

        internal MarkerGroup() {}

        public MarkerGroup Add<T>() where T : IAssemblyMarker
        {
            _assemblies.Add(typeof(T).Assembly);
            return this;
        }

        public static implicit operator Assembly[](MarkerGroup group)
        {
            return group._assemblies.ToArray();
        }
    }
}

public sealed record DependencyOptions : IDependencyOptions
{
    internal DependencyOptions() {}

    public DependencyFeature HashIdConverter { get; } = DependencyFeature.Default;
    public DependencyFeature IdGenerator { get; } = DependencyFeature.Default;
    public DependencyFeature PasswordHash { get; } = DependencyFeature.Default;
    public DependencyFeature PermissionVerifier { get; } = DependencyFeature.Default;
    public DependencyFeature RandomStringGenerator { get; } = DependencyFeature.Default;
}

public static class DependencyExtensions
{
    public static IServiceCollection AddSystemBase(this IServiceCollection services, ConfigureOptions<DependencyOptions>? configure = null)
    {
        var options = new DependencyOptions();
        configure?.Invoke(options);
        services.TryAddScoped<IPermissionVerifier, PermissionVerifier>(options.PermissionVerifier);
        services.TryAddSingleton<IHashIdConverter, HashIdConverter>(options.HashIdConverter);
        services.TryAddSingleton<IIdGenerator, IdGenerator>(options.IdGenerator);
        services.TryAddSingleton<IPasswordHash, PasswordHash>(options.PasswordHash);
        services.TryAddSingleton<IRandomStringGenerator, RandomStringGenerator>(options.RandomStringGenerator);
        return services;
    }
}