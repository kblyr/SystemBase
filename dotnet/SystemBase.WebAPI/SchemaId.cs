namespace SystemBase;

[AttributeUsage(AttributeTargets.Class)]
public class SchemaIdAttribute(string schemaId) : Attribute
{
    public string SchemaId { get; } = schemaId;
}

public static class SchemaIds
{
    public const string Success = "res:success";
    public const string Failed = "res:failed";
    public const string PermissionNotFound = "res:permissionnotfound";
    public const string ValidationFailed = "res:validationfailed";
    public const string VerifyPermissionFailed = "res:constd";
}