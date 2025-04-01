using AutoMapper;
using UserService.API.Grpc;
using UserService.Application.DTOs;

namespace UserService.API.Mappings
{
    public class GetUserByIdProfile : Profile
    {
        public GetUserByIdProfile()
        {
            CreateMap<UserDto, GetUserByIdResponse>();
        }
    }
}
