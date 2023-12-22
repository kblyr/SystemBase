namespace SystemBase;

public sealed record AccessToken
{
    public required string TokenString { get; init; }
    public DateTime ValidUntil { get; init; }
}

public sealed record AccessTokenGeneratorPayload
{
    public int Id { get; init; }
    public required string Username { get; init; }
    public required string FullName { get; init; }
}

public interface IAccessTokenGenerator
{
    AccessToken Generate(AccessTokenGeneratorPayload payload);
}