namespace SystemBase;

public record PermissionId 
{
    public string Id { get; set; } = "";

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

public static class PermissionIds
{
    
}