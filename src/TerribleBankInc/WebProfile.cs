using AutoMapper;
using TerribleBankInc.Models.Dtos;
using TerribleBankInc.Models.Entities;
using TerribleBankInc.Models.ViewModels;

namespace TerribleBankInc;

public class WebProfile : Profile
{
    public WebProfile()
    {
        CreateMap<Client, ClientUser>().ForMember(dest => dest.ClientId, opt => opt.MapFrom(src => src.ID));

        CreateMap<RegisterViewModel, Client>();

        CreateMap<Client, ClientViewModel>();
        CreateMap<ClientViewModel, Client>();

        CreateMap<NewBankAccountRequestViewModel, BankAccount>();
        CreateMap<BankTransactionViewModel, BankTransaction>();

        CreateMap<User, UserProfileViewModel>();
    }
}