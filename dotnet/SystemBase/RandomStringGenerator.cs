using System.Security.Cryptography;
using System.Text;

namespace SystemBase;

public interface IRandomStringGenerator
{
    string Generate(int length);
    string Generate(int length, char[] chars);
}

sealed class RandomStringGenerator : IRandomStringGenerator
{
    static readonly int _blockSize = 4;
    static readonly char[] _chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();

    public string Generate(int length) => Generate(length, _chars);

    public string Generate(int length, char[] chars)
    {
        if (length <= 0 || chars is { Length: 0 })
        {
            return "";
        }

        var randomData = RandomNumberGenerator.GetBytes(_blockSize * length);
        var builder = new StringBuilder();

        for (int resultIdx = 0; resultIdx < length; resultIdx++)
        {
            builder.Append(chars[BitConverter.ToUInt32(randomData, _blockSize * resultIdx) % chars.Length]);
        }

        return builder.ToString();
    }
}