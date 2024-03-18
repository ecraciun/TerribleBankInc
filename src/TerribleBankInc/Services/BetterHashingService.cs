using System;
using System.Security.Cryptography;
using System.Text;
using TerribleBankInc.Services.Interfaces;

namespace TerribleBankInc.Services;

public class BetterHashingService : IHashingService
{
    private const int SALT_SIZE = 32; // size in bytes
    private const int HASH_SIZE = 32; // size in bytes
    private const int ITERATIONS = 200000; // number of pbkdf2 iterations
    private string DEFAULT_SALT = "a+z21EScP3CsH/1G/EkpyVEo+S+ldyyyMYjeahd+8Lc=";

    public string GetHash(string input)
    {
        return GetHash(input, DEFAULT_SALT);
    }

    public string GetHash(string input, string salt)
    {
        byte[] saltBytes = Convert.FromBase64String(salt);
        // Generate the hash
        Rfc2898DeriveBytes pbkdf2 = new(input, saltBytes, ITERATIONS);
        byte[] hashBytes = pbkdf2.GetBytes(HASH_SIZE);
        return Convert.ToBase64String(hashBytes);
    }

    public (string Hash, string Salt) GenerateHashAndSalt(string input)
    {
        // Generate a salt
        RNGCryptoServiceProvider provider = new();
        byte[] salt = new byte[SALT_SIZE];
        provider.GetBytes(salt);
        string saltString = Convert.ToBase64String(salt);

        return (GetHash(input, saltString), saltString);
    }
}
