using System.Collections.Generic;
using System.Threading.Tasks;
using TerribleBankInc.Models.Entities;
using TerribleBankInc.Models.ViewModels;

namespace TerribleBankInc.Services.Interfaces;

public interface IBankAccountService
{
    Task<BankAccount> GetById(int id);
    Task<BankAccount> GetByAccountNumber(string accountNumber);
    Task<bool> RequestNewBankAccount(NewBankAccountRequestViewModel newAccount);
    Task<bool> ApproveAccount(int id);
    Task<bool> BlockAccount(int id);
    Task<bool> RejectAccount(int id, string reason);
    Task<bool> EnableAccount(int id);
    Task<bool> UpdateBalance(int id, decimal delta);
    Task<List<BankAccount>> GetAllAccountsForClient(int clientId);
    Task<List<BankAccount>> GetDisabledAccounts();
    Task<List<BankAccount>> GetPendingAccounts();
}
