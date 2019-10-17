using AutoMapper;
using System.Threading.Tasks;
using TerribleBankInc.Models;
using TerribleBankInc.Models.Entities;
using TerribleBankInc.Models.ViewModels;
using TerribleBankInc.Repositories;
using TerribleBankInc.Repositories.Interfaces;
using TerribleBankInc.Services.Interfaces;

namespace TerribleBankInc.Services
{
    public class ClientService : IClientService
    {
        private readonly IBaseRepository<Client> _clientRepository;
        private readonly IMapper _mapper;

        public ClientService(IBaseRepository<Client> clientRepository, IMapper mapper)
        {
            _clientRepository = clientRepository;
            _mapper = mapper;
        }

        public async Task<ClientViewModel> Get(int id)
        {
            var client = await _clientRepository.FindAsync(id);

            return _mapper.Map<ClientViewModel>(client);
        }

        public async Task<ClientViewModel> Update(ClientViewModel clientViewModel)
        {
            var client = _mapper.Map<Client>(clientViewModel);
            var result = await _clientRepository.UpdateAsync(client);
            return _mapper.Map<ClientViewModel>(result);
        }
    }
}