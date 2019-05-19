using AutoMapper;
using WalletIO.Dtos;
using WalletIO.Entities;

namespace WalletIO.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>();
        }
    }
}