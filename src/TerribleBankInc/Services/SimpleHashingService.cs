using System.Linq;
using System.Security.Cryptography;
using System.Text;
using TerribleBankInc.Services.Interfaces;

namespace TerribleBankInc.Services
{
    public class SimpleHashingService : IHashingService
    {
        public string GetHash(string input)
        {
            SHA1 veryStrongEncryptor = new SHA1CryptoServiceProvider();
            var hash = veryStrongEncryptor.ComputeHash(Encoding.UTF8.GetBytes(input));
            return string.Concat(hash.Select(b => b.ToString("x2")));
        }
    }
}