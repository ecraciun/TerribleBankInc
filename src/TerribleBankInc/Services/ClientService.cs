using System.Threading.Tasks;
using AutoMapper;
using TerribleBankInc.Models.Entities;
using TerribleBankInc.Models.ViewModels;
using TerribleBankInc.Repositories.Interfaces;
using TerribleBankInc.Services.Interfaces;

namespace TerribleBankInc.Services;

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
        Client client = await _clientRepository.FindAsync(id);

        return _mapper.Map<ClientViewModel>(client);
    }

    public async Task<ClientViewModel> Update(ClientViewModel clientViewModel)
    {
        Client client = _mapper.Map<Client>(clientViewModel);
        Client result = await _clientRepository.UpdateAsync(client);
        return _mapper.Map<ClientViewModel>(result);
    }
}
