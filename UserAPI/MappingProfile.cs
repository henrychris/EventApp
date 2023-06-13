using AutoMapper;
using Shared;
using Shared.RequestModels;
using Shared.ResponseModels;

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
