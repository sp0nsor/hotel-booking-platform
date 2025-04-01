using AutoMapper;
using UserService.Application.DTOs;
using UserService.Application.Requests;
using UserService.Infrastructure.Data.Entities;
using UserService.Infrastructure.Enums.Users;

namespace UserService.Application.Mappings
{
    public class UserEntityProfile : Profile
    {
        public UserEntityProfile()
        {
            CreateMap<RegisterUserRequest, UserEntity>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
                .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => (int)Roles.User));

            CreateMap<UpdateUserInfoRequest, UserEntity>();

            CreateMap<UserEntity, UserDto>();
        }
    }
}
