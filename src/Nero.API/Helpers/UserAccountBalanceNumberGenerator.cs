using System.Security.Cryptography;
using System.Text;

namespace Nero.Helpers;

public static class UserAccountBalanceNumberGenerator
{
    public static string GenerateUserAccountBalanceNumber()
    {
        var guidPart = Guid.NewGuid().ToString("N"); // Generates a Guid without hyphens
        var lettersPart = GenerateRandomLetters(6); // Generates a string of 6 random letters
        var hashPart = GenerateHash($"{guidPart}-{lettersPart}");
        return $"{guidPart}-{lettersPart}-{hashPart}";
    }

    private static string GenerateRandomLetters(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
        var random = new Random();
        var result = new char[length];

        for (int i = 0; i < length; i++)
        {
            result[i] = chars[random.Next(chars.Length)];
        }

        return new string(result);
    }

    private static string GenerateHash(string input)
    {
        using var sha256 = SHA256.Create();
        var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
        var hash = BitConverter.ToString(hashBytes)
            .Replace("-", "")
            .Substring(0, 8); // Take the first 8 characters of the hash

        return hash;
    }
}