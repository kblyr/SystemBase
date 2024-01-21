namespace SystemBase;

[AttributeUsage(AttributeTargets.Class)]
public class SchemaIdAttribute : Attribute
{
    public string SchemaId { get; }

    public SchemaIdAttribute(string schemaId)
    {
        SchemaId = schemaId;
    }
}

public static class SchemaIds
{
    public const string Success                 = "res:success";
    public const string Failed                  = "res:failed";
    public const string PermissionNotFound      = "res:permissionnotfound";
    public const string ValidationFailed        = "res:validationfailed";
    public const string VerifyPermissionFailed  = "res:verifypermissionfailed";
}