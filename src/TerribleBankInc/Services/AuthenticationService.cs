using AutoMapper;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TerribleBankInc.Models;
using TerribleBankInc.Models.Dtos;
using TerribleBankInc.Models.Entities;
using TerribleBankInc.Models.OperationResults;
using TerribleBankInc.Models.ViewModels;
using TerribleBankInc.Repositories;
using TerribleBankInc.Repositories.Interfaces;
using TerribleBankInc.Services.Interfaces;

namespace TerribleBankInc.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IBaseRepository<Client> _clientRepository;
        private readonly IBaseRepository<User> _userRepository;
        private readonly IMapper _mapper;

        public AuthenticationService(IBaseRepository<Client> clientRepository, IBaseRepository<User> userRepository, IMapper mapper)
        {
            _clientRepository = clientRepository;
            _userRepository = userRepository;
            _mapper = mapper;
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
                ClientUser = _mapper.Map<ClientUser>(client),
                IsSuccess = true
            };
        }

        public async Task<bool> ChangePasswordAsync(int userId, string oldPassword, string newPassword)
        {
            var user = await _userRepository.FindAsync(userId);
            if(user != null && user.HashedPassword.Equals(GetHash(oldPassword)))
            {
                user.HashedPassword = GetHash(newPassword);
                await _userRepository.UpdateAsync(user);
                return true;
            }
            return false;
        }

        public async Task<LoginResult> RegisterAsync(RegisterViewModel userVm)
        {
            var client = _mapper.Map<Client>(userVm);
            client = await _clientRepository.AddAsync(client);
            
            var user = new User
            {
                ClientId = client.ID,
                Username = client.Email,
                HashedPassword = GetHash(userVm.Password),
                IsAdmin = false
            };
            await _userRepository.AddAsync(user);

            return new LoginResult
            {
                IsSuccess = true,
                ClientUser = _mapper.Map<ClientUser>(client)
            };
        }

        private string GetHash(string input)
        {
            SHA1 veryStrongEncryptor = new SHA1CryptoServiceProvider();
            var hash = veryStrongEncryptor.ComputeHash(Encoding.UTF8.GetBytes(input));
            return string.Concat(hash.Select(b => b.ToString("x2")));
        }
    }
}