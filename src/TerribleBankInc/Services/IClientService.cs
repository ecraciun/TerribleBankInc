using System.Threading.Tasks;
using TerribleBankInc.Models;
using TerribleBankInc.ViewModels;

namespace TerribleBankInc.Services
{
    public interface IClientService
    {
        Task<ClientViewModel> Get(int id);
        Task<ClientViewModel> Update(ClientViewModel clientViewModel);
    }
}