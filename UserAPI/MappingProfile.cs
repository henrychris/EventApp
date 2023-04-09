using AutoMapper;
using Shared;
using Shared.DTO;

namespace UserAPI
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<RegisterRequest, User>();
            CreateMap<User, AuthResponse>();
        }
    }
}
