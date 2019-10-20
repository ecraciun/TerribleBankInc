using System.Collections.Generic;
using System.Threading.Tasks;
using TerribleBankInc.Models.Entities;
using TerribleBankInc.Models.ViewModels;

namespace TerribleBankInc.Services.Interfaces
{
    public interface IBankTransferService
    {
        Task<bool> Send(BankTransactionViewModel transactionViewModel);

        Task<List<BankTransaction>> GetAllForAccount(int accountId);
    }
}