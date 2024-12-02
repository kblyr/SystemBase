namespace SystemBase;

public interface IDependencyOptions {}

public record DependencyFeature
{
    public bool IsIncluded { get; protected set; }

    public DependencyFeature Include()
    {
        IsIncluded = true;
        return this;
    }

    public DependencyFeature Exclude()
    {
        IsIncluded = false;
        return this;
    }

    public static DependencyFeature Default() => new() { IsIncluded = true };

    public static implicit operator bool(DependencyFeature feature) => feature.IsIncluded;
}

public delegate void ConfigureDependencyOptions<T>(T options) where T : IDependencyOptions;

public interface IAssemblyMarker {}

public sealed class AssemblyMarker : IAssemblyMarker
{
    public static MarkerGroup Group()
    {
        return new();
    }

    public sealed class MarkerGroup
    {
        readonly HashSet<System.Reflection.Assembly> _assemblies = [];

        internal MarkerGroup() {}

        public MarkerGroup Add<T>() where T : IAssemblyMarker
        {
            _assemblies.Add(typeof(T).Assembly);
            return this;
        }

        public static implicit operator System.Reflection.Assembly[](MarkerGroup group) => [..group._assemblies];
    }
}