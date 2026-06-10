using Application.DTO;
using AutoMapper;
using System;
using System.Collections.Generic;


namespace Application.Mapper
{
    public class UserMappingProfile : Profile
    {

        public UserMappingProfile()
        {

            CreateMap<Domain.UserAgregate.User, UserDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.PasswordHash))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ReverseMap();
        }

    }
}
