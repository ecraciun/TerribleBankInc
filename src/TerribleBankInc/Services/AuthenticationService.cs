using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TerribleBankInc.Models;
using TerribleBankInc.Repositories;
using TerribleBankInc.ViewModels;

namespace TerribleBankInc.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IBaseRepository<Client> _clientRepository;
        private readonly IBaseRepository<User> _userRepository;

        public AuthenticationService(IBaseRepository<Client> clientRepository, IBaseRepository<User> userRepository)
        {
            _clientRepository = clientRepository;
            _userRepository = userRepository;
        }

        public async Task<LoginResult> LoginAsync(string username, string password)
        {
            var user = _userRepository.GetAll().FirstOrDefault(x => x.Username == username);
            if (user == null)
            {
                return new LoginResult
                {
                    IsSuccess = false,
                    Message = "No user registered with provided email."
                };
            }

            var hash = GetHash(password);
            if (!hash.Equals(user.HashedPassword))
            {
                return new LoginResult
                {
                    IsSuccess = false,
                    Message = "Incorrect password."
                };
            }

            // Yes yes, .Include would have done the trick in EF
            var client = await _clientRepository.FindAsync(user.ClientId);

            return new LoginResult
            {
                ClientUser = new ClientUser
                {
                    ClientId = client.ID,
                    UserId = user.ID,
                    Email = client.Email,
                    IsAdmin = user.IsAdmin,
                    FirstName = client.FirstName,
                    LastName = client.LastName
                },
                IsSuccess = true
            };
        }

        public async Task<bool> ChangePasswordAsync(int userId, string oldPassword, string newPassword)
        {
            throw new NotImplementedException();
        }

        public async Task<LoginResult> RegisterAsync(RegisterViewModel user)
        {
            throw new NotImplementedException();
        }

        private string GetHash(string input)
        {
            SHA1 veryStrongEncryptor = new SHA1CryptoServiceProvider();
            var hash = veryStrongEncryptor.ComputeHash(Encoding.UTF8.GetBytes(input));
            return string.Concat(hash.Select(b => b.ToString("x2")));
        }
    }
}