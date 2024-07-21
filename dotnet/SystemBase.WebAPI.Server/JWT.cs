namespace SystemBase;

public sealed record JWTOptions
{
    public static string CONFIGKEY { get; set; } = "SystemBase:JWT";

    public string Key { get; set; } = "";
    public TimeSpan Expiration { get; set; } = TimeSpan.FromHours(1);
    public string Issuer { get; set; } = "";
    public string Audience { get; set; } = "";
    public bool ValidateIssuer { get; set; } = false;
    public bool ValidateAudience { get; set; } = false;
    public bool ValidateLifetime { get; set; } = true;
    public bool ValidateIssuerSigningKey { get; set; } = true;
}