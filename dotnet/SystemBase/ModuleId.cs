namespace SystemBase;

public record ModuleId
{
    string? _id;
    public string Id 
    {
        get => _id ?? StringUtilities.ToSnakeCase(GetType().Name);
        set => _id = value;
    }
    
    public DomainId? Domain { get; set; }
    public ModuleId? Parent { get; set; }

    public string[] Ids => [..Parent?.Ids ?? [], Id];

    public static string Separator { get; set; } = "/";
    public static string DomainSeparator { get; set; } = ":";

    static readonly KeyedCachedData<Type, ModuleId> _cached = new();

    public static ModuleId GetCached<T>() where T : ModuleId, new()
    {
        var type = typeof(T);
        if (_cached.TryGet(type, out var id))
        {
            return id;
        }

        return _cached.Set(type, new T());
    }
}

public record ModuleId<TDomain> : ModuleId where TDomain : DomainId, new()
{
    public ModuleId() => Domain = DomainId.GetCached<TDomain>();
}

public record SubModuleId<TParent> : ModuleId where TParent : ModuleId, new()
{
    public SubModuleId()
    {
        Parent = GetCached<TParent>();
        Domain = Parent.Domain;
    }
}

public static class DomainIds
{
    public record Accounts : DomainId
    {
        public record User : DomainId<Accounts> {}

        public record Role : DomainId<Accounts> {}
    }
}

public static class ModuleIds
{
    public static class Accounts
    {
        public record UserManagement : ModuleId<DomainIds.Accounts.User>
        {
            public record PasswordManagement : SubModuleId<UserManagement> {}
        }

        public record RoleManagement : ModuleId<DomainIds.Accounts.Role> {}
    }
}