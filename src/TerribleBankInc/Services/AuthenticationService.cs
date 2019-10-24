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
        private const int ForgotTokenLifeInMinutes = 5;

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
            var user = (await _userRepository.Get(x => x.Username == username, nameof(User.Client))).FirstOrDefault();
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

            return new LoginResult
            {
                ClientUser = _mapper.Map<ClientUser>(user.Client),
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

        public async Task<ForgotPasswordResult> CreatePasswordForgetToken(string username)
        {
            var user = (await _userRepository.Get(x => x.Username == username)).FirstOrDefault();
            if (user == null)
            {
                return new ForgotPasswordResult
                {
                    IsSuccess = false,
                    Message = "No user registered with provided email."
                };
            }

            user.ForgotPasswordToken = Guid.NewGuid().ToString();
            user.ForgotPasswordExpiration = DateTime.UtcNow.AddMinutes(ForgotTokenLifeInMinutes);
            await _userRepository.UpdateAsync(user);

            return new ForgotPasswordResult
            {
                IsSuccess = true,
                ForgotPasswordExpiration = user.ForgotPasswordExpiration.Value,
                ForgotPasswordToken = user.ForgotPasswordToken
            };
        }

        public async Task<bool> IsForgotPasswordTokenValid(string token)
        {
            var user = (await _userRepository.Get(x => x.ForgotPasswordToken == token)).FirstOrDefault();
            if (user == null) return false;

            if (DateTime.UtcNow > user.ForgotPasswordExpiration.Value) return false;

            return true;
        }

        public async Task<OperationResult> ResetPasswordWithToken(string token, string newPassword)
        {
            var user = (await _userRepository.Get(x => x.ForgotPasswordToken == token)).FirstOrDefault();
            if (user == null || user.ForgotPasswordExpiration.Value < DateTime.UtcNow)
            {
                return new OperationResult
                {
                    IsSuccess = false,
                    Message = "Invalid token"
                };
            }

            user.HashedPassword = GetHash(newPassword);
            user.ForgotPasswordExpiration = null;
            user.ForgotPasswordToken = null;
            await _userRepository.UpdateAsync(user);

            return new OperationResult
            {
                IsSuccess = true
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