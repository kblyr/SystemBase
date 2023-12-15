using System.Security.Cryptography;

namespace SystemBase;

static class PASSWORDHASH_GLOBAL
{
    public static readonly int SaltLength = 32;
    public static readonly int Iterations = 1_000;
    public static readonly int HashLength = 128;
    public static readonly HashAlgorithmName AlgorithmName = HashAlgorithmName.SHA512;
}

public interface IPasswordHash
{
    IPasswordHashComputeResult Compute(string password);
    IPasswordHashComputeResult Compute(string password, string salt);
    IPasswordHashVerifyResult Verify(string hashedPassword, string password, string salt);
}

public interface IPasswordHashComputeResult { }

public interface IPasswordHashVerifyResult { }

public sealed record PasswordHashComputeSuccessResult : IPasswordHashComputeResult
{
    public required string HashedPassword { get; init; }
    public required string Salt { get; init; }

    public static PasswordHashComputeSuccessResult Instance(string hashedPassword, string salt) => new()
    {
        HashedPassword = hashedPassword,
        Salt = salt
    };
}

public sealed record PasswordHashComputeErrorResult : IPasswordHashComputeResult
{
    public Exception? Exception { get; init; }
    public required string ErrorMessage { get; init; }

    public static PasswordHashComputeErrorResult Instance(Exception exception) => new()
    {
        Exception = exception,
        ErrorMessage = exception.Message
    };

    public static PasswordHashComputeErrorResult Instance(string errorMessage) => new()
    {
        ErrorMessage = errorMessage
    };

    public static PasswordHashComputeErrorResult Instance(Exception exception, string errorMessage) => new()
    {
        Exception = exception,
        ErrorMessage = errorMessage
    };
}

public sealed record PasswordHashVerifySuccessResult : IPasswordHashVerifyResult
{
    public bool IsVerified { get; init; }

    public static PasswordHashVerifySuccessResult Instance(bool isVerified) => new() { IsVerified = isVerified };
}

public sealed record PasswordHashVerifyErrorResult : IPasswordHashVerifyResult
{
    public Exception? Exception { get; init; }
    public required string ErrorMessage { get; init; }

    public static PasswordHashVerifyErrorResult Instance(Exception exception) => new()
    {
        Exception = exception,
        ErrorMessage = exception.Message
    };

    public static PasswordHashVerifyErrorResult Instance(string errorMessage) => new()
    {
        ErrorMessage = errorMessage
    };

    public static PasswordHashVerifyErrorResult Instance(Exception exception, string errorMessage) => new()
    {
        Exception = exception,
        ErrorMessage = errorMessage
    };
}

sealed class PasswordHash : IPasswordHash
{
    static Rfc2898DeriveBytes InitializeAlgorithm(string password, byte[] salt)
    {
        return new Rfc2898DeriveBytes(password, salt, PASSWORDHASH_GLOBAL.Iterations, PASSWORDHASH_GLOBAL.AlgorithmName);
    }

    static string Compute(string password, byte[] salt)
    {
        using var algorithm = InitializeAlgorithm(password, salt);
        return ConvertDataToText(algorithm.GetBytes(PASSWORDHASH_GLOBAL.HashLength));
    }

    public IPasswordHashComputeResult Compute(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
        {
            return PasswordHashComputeErrorResult.Instance("Password cannot be null or white-space");
        }

        var salt = GenerateSalt();
        return PasswordHashComputeSuccessResult.Instance(Compute(password, salt), ConvertDataToText(salt));
    }

    public IPasswordHashComputeResult Compute(string password, string salt)
    {
        if (string.IsNullOrWhiteSpace(password))
        {
            return PasswordHashComputeErrorResult.Instance("Password cannot be null or white-space");
        }

        if (string.IsNullOrWhiteSpace(salt))
        {
            return PasswordHashComputeErrorResult.Instance("Salt cannot be null or white-space");
        }

        return PasswordHashComputeSuccessResult.Instance(Compute(password, ConvertTextToData(salt)), salt);
    }

    public IPasswordHashVerifyResult Verify(string hashedPassword, string password, string salt)
    {
        if (string.IsNullOrWhiteSpace(hashedPassword))
        {
            return PasswordHashVerifyErrorResult.Instance("Hashed password cannot be null or white-space");
        }

        if (string.IsNullOrWhiteSpace(password))
        {
            return PasswordHashVerifyErrorResult.Instance("Password cannot be null or white-space");
        }

        if (string.IsNullOrWhiteSpace(salt))
        {
            return PasswordHashVerifyErrorResult.Instance("Salt cannot be null or white-space");
        }

        return PasswordHashVerifySuccessResult.Instance(string.Equals(hashedPassword, Compute(password, ConvertTextToData(salt))));
    }

    static byte[] GenerateSalt()
    {
        return RandomNumberGenerator.GetBytes(PASSWORDHASH_GLOBAL.SaltLength);
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