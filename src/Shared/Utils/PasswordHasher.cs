using System;

namespace Shared.Utils;

public class PasswordHasher
{
    public static string HashWithSHA256(string input)
    {
        var bytes = System.Text.Encoding.UTF8.GetBytes(input);
        var hash = System.Security.Cryptography.SHA256.HashData(bytes);
        return Convert.ToBase64String(hash);
    }
}
