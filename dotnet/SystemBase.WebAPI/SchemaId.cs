namespace SystemBase;

[AttributeUsage(AttributeTargets.Class)]
public class SchemaIdAttribute(string schemaId) : Attribute
{
    public string SchemaId { get; } = schemaId;
}

public static class SchemaIds
{
    public static string Success { get; set; } = "res:success";
    public static string Failed { get; set; } = "res:failed";
    public static string PermissionNotFound { get; set; } = "res:permissionnotfound";
    public static string ValidationFailed { get; set; } = "res:validationfailed";
    public static string VerifyPermissionFailed { get; set; } = "res:verifypermissionfailed";
}