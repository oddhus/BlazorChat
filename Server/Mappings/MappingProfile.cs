using AutoMapper;
using BlazorChat.Server.Models;
using BlazorChat.Shared.Dtos;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Contact, ContactDto>();
        CreateMap<ContactDto, Contact>();

        CreateMap<User, UserReadDto>();
        CreateMap<UserUpdateDto, User>();
        CreateMap<User, UserSettingsDto>();
        CreateMap<User, ContactDto>();

        CreateMap<RegisterDto, Account>();
        CreateMap<RegisterDto, User>();
        CreateMap<RegisterDto, LoginDto>();

        CreateMap<Account, AccountDto>();

        CreateMap<MessageDto, Message>();
        CreateMap<Message, MessageDto>();

        CreateMap<Chat, ChatDto>();
    }
}