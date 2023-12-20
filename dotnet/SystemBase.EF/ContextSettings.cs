namespace SystemBase;

public sealed record ContextSettings : IDependencyFeatureSettings
{
    public Action<DbContextOptionsBuilder>? ConfigureOptionsBuilder { get; set; }
}