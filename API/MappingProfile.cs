﻿using AutoMapper;
using ErrandPay_Test.Shared.DTOs;
using Shared;

namespace API
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