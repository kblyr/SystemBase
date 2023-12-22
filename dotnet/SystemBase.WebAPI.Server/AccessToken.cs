namespace SystemBase;

sealed class AccessTokenGenerator : IAccessTokenGenerator
{
    readonly IOptions<JWTOptions> _jwt;
    readonly IHashIdConverter _hashIdConverter;

    public AccessTokenGenerator(IOptions<JWTOptions> jwt, IHashIdConverter hashIdConverter)
    {
        _jwt = jwt;
        _hashIdConverter = hashIdConverter;
    }

    public AccessToken Generate(AccessTokenGeneratorPayload payload)
    {
        var expires = DateTime.UtcNow.Add(_jwt.Value.Expiration);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.User.Id, _hashIdConverter.FromInt32(payload.Id)),
                new Claim(ClaimTypes.User.Username, payload.Username),
                new Claim(ClaimTypes.User.FullName, payload.FullName)
            }),
            SigningCredentials = _jwt.Value.GetSigningCredentials(),
            Expires = expires,
            Issuer = _jwt.Value.Issuer,
            Audience = _jwt.Value.Audience
        };
        var tokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return new AccessToken
        {
            TokenString = tokenHandler.WriteToken(token),
            ValidUntil = expires
        };
    }
}