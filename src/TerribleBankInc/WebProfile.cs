using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TerribleBankInc.Models;
using TerribleBankInc.ViewModels;

namespace TerribleBankInc
{
    public class WebProfile : Profile
    {
        public WebProfile()
        {
            CreateMap<Client, ClientUser>().ForMember(dest => dest.ClientId, opt => opt.MapFrom(src => src.ID));

            CreateMap<RegisterViewModel, Client>();
        }
    }
}
