using AutoMapper;
using Shared;

namespace EventAPI
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateEventDTO, Event>();
        }
    }
}
