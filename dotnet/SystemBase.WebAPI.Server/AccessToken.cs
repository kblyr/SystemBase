using System.IdentityModel.Tokens.Jwt;

namespace SystemBase;

sealed class AccessTokenGenerator(IOptions<JWTOptions> jwt) : IAccessTokenGenerator
{
    readonly IOptions<JWTOptions> _jwt = jwt;

    public AccessToken Generate(IDictionary<string, string> payload)
    {
        var expires = DateTimeOffset.Now.Add(_jwt.Value.Expiration);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(payload.Select(item => new Claim(item.Key, item.Value))),
            SigningCredentials = _jwt.Value.GetSigningCredentials(),
            Expires = expires.DateTime,
            Issuer = _jwt.Value.Issuer,
            Audience = _jwt.Value.Audience
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