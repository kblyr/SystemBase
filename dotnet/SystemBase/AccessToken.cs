namespace SystemBase;

public sealed record AccessToken
{
    public required string Token { get; init; }
    public DateTimeOffset ExpiresOn { get; init; }
}

public interface IAccessTokenGenerator
{
    AccessToken Generate(IDictionary<string, string> payload);
}
