using AutoMapper;
using BlazorChat.Server.Models;
using BlazorChat.Shared.Dtos;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Contact, ContactReadDto>();
        CreateMap<User, UserReadDto>();
        CreateMap<UserUpdateDto, User>();
    }
}