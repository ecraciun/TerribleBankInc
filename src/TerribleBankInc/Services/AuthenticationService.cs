using AutoMapper;
using System;
using System.Diagnostics;
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
        private readonly IHashingService _hashingService;

        public AuthenticationService(IBaseRepository<Client> clientRepository, IBaseRepository<User> userRepository, IMapper mapper, IHashingService hashingService)
        {
            _clientRepository = clientRepository;
            _userRepository = userRepository;
            _mapper = mapper;
            _hashingService = hashingService;
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

            Stopwatch sw = new Stopwatch();
            sw.Start();
            var hash = _hashingService.GetHash(password);
            sw.Stop();

            Debug.WriteLine($"\n\n\nHASHING TIME: {sw.ElapsedMilliseconds}");

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
            if(user != null && user.HashedPassword.Equals(_hashingService.GetHash(oldPassword)))
            {
                user.HashedPassword = _hashingService.GetHash(newPassword);
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
                HashedPassword = _hashingService.GetHash(userVm.Password),
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

            user.HashedPassword = _hashingService.GetHash(newPassword);
            user.ForgotPasswordExpiration = null;
            user.ForgotPasswordToken = null;
            await _userRepository.UpdateAsync(user);

            return new OperationResult
            {
                IsSuccess = true
            };
        }

        public async Task<User> GetUserByClientId(int id)
        {
            return (await _userRepository.Get(x => x.ClientId == id)).FirstOrDefault();
        }
    }
}