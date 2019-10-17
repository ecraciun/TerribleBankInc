using System.Threading.Tasks;
using TerribleBankInc.Models.ViewModels;

namespace TerribleBankInc.Services.Interfaces
{
    public interface IBankTransferService
    {
        Task<bool> Send(BankTransactionViewModel transactionViewModel);
    }
}