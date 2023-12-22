namespace SystemBase;

public static class JwtOptionsExtensions
{
    public static SymmetricSecurityKey GetSymmetricSecurityKey(this JWTOptions options) => new(System.Text.Encoding.UTF8.GetBytes(options.Key));

    public static SigningCredentials GetSigningCredentials(this JWTOptions options) => options.GetSymmetricSecurityKey().ToSigningCredentials();
}