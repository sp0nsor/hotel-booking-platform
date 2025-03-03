using AutoMapper;
using HotelService.API.Contracts.Hotels;
using HotelService.API.Contracts.Rooms;
using HotelService.Application.RequestHandlers.Commands.Hotel.Create;
using HotelService.Application.RequestHandlers.Commands.Hotel.Update;
using HotelService.Application.RequestHandlers.Commands.Room.Create;
using HotelService.Application.RequestHandlers.Commands.Room.Update;

namespace HotelService.API.Mappings
{
    public class RequestProfile : Profile
    {
        public RequestProfile()
        {
            CreateMap<CreateRoomRequest, CreateRoomCommand>();
            CreateMap<UpdateRoomRequest, UpdateRoomCommand>();

            CreateMap<CreateHotelRequest, CreateHotelCommand>();
            CreateMap<UpdateHotelRequest, UpdateHotelCommand>();
        }
    }
}
