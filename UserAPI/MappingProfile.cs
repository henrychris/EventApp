using AutoMapper;
using Shared;
using Shared.DTO;

namespace UserAPI
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<RegisterRequest, User>()
                .ForMember(dest => dest.PasswordHash, act => act.MapFrom(src => src.Password));
        }
    }
}
