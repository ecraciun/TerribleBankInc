using System.Threading.Tasks;
using TerribleBankInc.Models;
using TerribleBankInc.ViewModels;

namespace TerribleBankInc.Services
{
    public interface IAuthenticationService
    {
        Task<LoginResult> LoginAsync(string username, string password);
        Task<bool> ChangePasswordAsync(int userId, string oldPassword, string newPassword);
        Task<LoginResult> RegisterAsync(RegisterViewModel user);
    }
}