namespace SystemBase;

public record DomainId
{
    string? _id;
    public string Id 
    {
        get => _id ?? StringUtilities.ToSnakeCase(GetType().Name);
        set => _id = value;
    }

    public DomainId? Parent { get; set; }

    public string[] Ids => [..Parent?.Ids ?? [], Id];

    public virtual string IdsString => string.Join(Separator, Ids);

    public static string Separator { get; set; } = "/";

    static readonly KeyedCachedData<Type, DomainId> _cached = new();
    
    public static DomainId GetCached<T>() where T : DomainId, new()
    {
        var type = typeof(T);
        if (_cached.TryGet(type, out var id))
        {
            return id;
        }

        return _cached.Set(type, new T());
    }
}

public record DomainId<TParent> : DomainId where TParent : DomainId, new()
{
    public DomainId() => Parent = GetCached<TParent>();
}