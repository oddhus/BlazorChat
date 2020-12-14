using AutoMapper;
using BlazorChat.Server.Models;
using BlazorChat.Shared.Dtos;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Contact, ContactReadDto>(); //Map from Developer Object to DeveloperDTO Object
        CreateMap<User, UserReadDto>(); //Map from Developer Object to DeveloperDTO Object
    }
}