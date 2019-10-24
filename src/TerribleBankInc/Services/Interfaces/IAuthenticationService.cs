using System.Threading.Tasks;
using TerribleBankInc.Models.OperationResults;
using TerribleBankInc.Models.ViewModels;

namespace TerribleBankInc.Services.Interfaces
{
    public interface IAuthenticationService
    {
        Task<LoginResult> LoginAsync(string username, string password);
        Task<bool> ChangePasswordAsync(int userId, string oldPassword, string newPassword);
        Task<LoginResult> RegisterAsync(RegisterViewModel user);
        Task<ForgotPasswordResult> CreatePasswordForgetToken(string username);
        Task<bool> IsForgotPasswordTokenValid(string token);
        Task<OperationResult> ResetPasswordWithToken(string token, string newPassword);
    }
}