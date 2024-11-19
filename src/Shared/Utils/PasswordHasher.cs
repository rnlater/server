namespace Shared.Utils;

public class PasswordHasher
{
    /// <summary>
    /// Hash a string with SHA256 algorithm.
    /// </summary>
    /// <param name="input"></param>
    /// <returns>return the hashed string</returns>
    public static string HashWithSHA256(string input)
    {
        var bytes = System.Text.Encoding.UTF8.GetBytes(input);
        var hash = System.Security.Cryptography.SHA256.HashData(bytes);
        return Convert.ToBase64String(hash);
    }
}
