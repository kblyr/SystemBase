using System.IdentityModel.Tokens.Jwt;

namespace SystemBase;

sealed class AccessTokenGenerator(IOptions<JWTOptions> jwt) : IAccessTokenGenerator
{
    public AccessToken Generate(IDictionary<string, string> payload)
    {
        var expires = DateTimeOffset.Now.Add(jwt.Value.Expiration);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(payload.Select(item => new Claim(item.Key, item.Value))),
            SigningCredentials = jwt.Value.GetSigningCredentials(),
            Expires = expires.DateTime,
            Issuer = jwt.Value.Issuer,
            Audience = jwt.Value.Audience
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return new AccessToken
        {
            Token = tokenHandler.WriteToken(token),
            ExpiresOn = expires
        };
    }
}