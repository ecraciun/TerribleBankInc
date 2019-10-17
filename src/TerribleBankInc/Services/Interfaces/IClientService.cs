using System.Threading.Tasks;
using TerribleBankInc.Models.ViewModels;

namespace TerribleBankInc.Services.Interfaces
{
    public interface IClientService
    {
        Task<ClientViewModel> Get(int id);
        Task<ClientViewModel> Update(ClientViewModel clientViewModel);
    }
}