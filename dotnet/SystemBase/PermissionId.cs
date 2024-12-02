namespace SystemBase;

public record PermissionId 
{
    string? _id;
    public string Id 
    {
        get => _id ?? StringUtilities.ToSnakeCase(GetType().Name);
        set => _id = value;
    }
    

    static readonly KeyedCachedData<Type, PermissionId> _cached = new();

    public static PermissionId GetCached<T>() where T : PermissionId, new()
    {
        var type = typeof(T);
        if (_cached.TryGet(type, out var id))
        {
            return id;
        }
        
        return _cached.Set(type, new T());
    }
}