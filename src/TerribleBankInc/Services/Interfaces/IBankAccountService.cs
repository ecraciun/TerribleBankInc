using System.Threading.Tasks;
using TerribleBankInc.Models.Entities;

namespace TerribleBankInc.Services.Interfaces
{
    public interface IBankAccountService
    {
        Task<BankAccount> GetById(int id);
        Task<BankAccount> GetByAccountNumber(string accountNumber);
        Task<bool> RequestNewBankAccount(BankAccount newAccount);
        Task<bool> ApproveAccount(int id);
        Task<bool> DenyAccount(int id);
        Task<bool> BlockAccount(int id);
        Task<bool> EnableAccount(int id);
        Task<bool> UpdateBalance(int id, decimal delta);
    }
}