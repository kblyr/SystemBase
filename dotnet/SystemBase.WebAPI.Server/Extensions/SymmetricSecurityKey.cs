namespace SystemBase;

public static class SymmetricSecurityKeyExtensions
{
    public static SigningCredentials ToSigningCredentials(this SymmetricSecurityKey key) => new(key, SecurityAlgorithms.HmacSha512Signature);
}