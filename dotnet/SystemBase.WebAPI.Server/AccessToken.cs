namespace SystemBase;

sealed class AccessTokenGenerator : IAccessTokenGenerator
{
    readonly IOptions<JWTOptions> _jwt;

    public AccessTokenGenerator(IOptions<JWTOptions> jwt)
    {
        _jwt = jwt;
    }

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
        var tokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return new AccessToken
        {
            Token = tokenHandler.WriteToken(token),
            ExpiresOn = expires
        };
    }
}