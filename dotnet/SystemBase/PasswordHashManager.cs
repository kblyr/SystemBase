using System.Security.Cryptography;

namespace SystemBase;

static class PASSWORDHASHMANAGER_GLOBAL 
{
    public static readonly int SaltLength = 32;
    public static readonly int Iterations = 1000;
    public static readonly int HashLength = 128;
    public static readonly HashAlgorithmName AlgorithmName = HashAlgorithmName.SHA512;
}

public interface IPasswordHashManager
{
    IResponse Compute(string password);
    IResponse Compute(string password, string salt);
    IResponse Verify(string hashedPassword, string salt, string inputPassword);
}

[ResponseType<ResponseTypes.PasswordHashManager.Computed>]
public sealed record PasswordHashManager_Computed(string HashPassword, string Salt) : ISuccessResponse;

[ResponseType<ResponseTypes.PasswordHashManager.IncorrectPassword>]
public sealed record PasswordHashManager_IncorrectPassword : IFailureResponse
{
    public static readonly PasswordHashManager_IncorrectPassword Instance = new();
}

[ResponseType<ResponseTypes.PasswordHashManager.InvalidHashedPassword>]
public sealed record PasswordHashManager_InvalidHashedPassword : IFailureResponse
{
    public static readonly PasswordHashManager_InvalidHashedPassword Instance = new();
}

[ResponseType<ResponseTypes.PasswordHashManager.InvalidInputPassword>]
public sealed record PasswordHashManager_InvalidInputPassword : IFailureResponse
{
    public static readonly PasswordHashManager_InvalidInputPassword Instance = new();
}

[ResponseType<ResponseTypes.PasswordHashManager.InvalidSalt>]
public sealed record PasswordHashManager_InvalidSalt : IFailureResponse
{
    public static readonly PasswordHashManager_InvalidSalt Instance = new();
}

[ResponseType<ResponseTypes.PasswordHashManager.Verified>]
public sealed record PasswordHashManager_Verified : ISuccessResponse
{
    public static readonly PasswordHashManager_Verified Instance = new();
}

public sealed class PasswordHashManager : IPasswordHashManager
{
    static Rfc2898DeriveBytes InitializeAlgorithm(string password, byte[] salt)
    {
        return new Rfc2898DeriveBytes(password, salt, PASSWORDHASHMANAGER_GLOBAL.Iterations, PASSWORDHASHMANAGER_GLOBAL.AlgorithmName);
    }

    static string Compute(string password, byte[] salt)
    {
        using var algorithm = InitializeAlgorithm(password, salt);
        return ConvertDataToText(algorithm.GetBytes(PASSWORDHASHMANAGER_GLOBAL.HashLength));
    }

    public IResponse Compute(string password)
    {
        if (password is null)
        {
            return PasswordHashManager_InvalidInputPassword.Instance;
        }

        var salt = GenerateSalt();
        return new PasswordHashManager_Computed(Compute(password, salt), ConvertDataToText(salt));
    }

    public IResponse Compute(string password, string salt)
    {
        if (password is null)
        {
            return PasswordHashManager_InvalidInputPassword.Instance;
        }

        if (salt is null)
        {
            return PasswordHashManager_InvalidSalt.Instance;
        }

        return new PasswordHashManager_Computed(Compute(password, ConvertTextToData(salt)), salt);
    }

    public IResponse Verify(string hashedPassword, string salt, string inputPassword)
    {
        if (hashedPassword is null)
        {
            return PasswordHashManager_InvalidHashedPassword.Instance;
        }

        if (salt is null)
        {
            return PasswordHashManager_InvalidSalt.Instance;
        }

        if (inputPassword is null)
        {
            return PasswordHashManager_InvalidInputPassword.Instance;
        }

        if (hashedPassword.Length == 0 && salt.Length == 0 && inputPassword.Length == 0)
        {
            return PasswordHashManager_Verified.Instance;
        }

        return string.Equals(hashedPassword, Compute(inputPassword, ConvertTextToData(salt)))
            ? PasswordHashManager_Verified.Instance
            : PasswordHashManager_IncorrectPassword.Instance;
    }

    static byte[] GenerateSalt()
    {
        return RandomNumberGenerator.GetBytes(PASSWORDHASHMANAGER_GLOBAL.SaltLength);
    }

    static byte[] ConvertTextToData(string text)
    {
        return Convert.FromBase64String(text);
    }

    static string ConvertDataToText(byte[] data)
    {
        return Convert.ToBase64String(data);
    }
}