using AutoMapper;
using MoneyManager.Application.Functions.Records;
using MoneyManager.Application.Functions.Users.Commands.LoginUser;
using MoneyManager.Domain.Authentication;
using MoneyManager.Domain.Entities;

namespace MoneyManager.Application.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Record, CreateRecordCommand>()
                .ReverseMap();

            CreateMap<Record, RecordDto>()
                .ReverseMap();

            CreateMap<LoginUser, LoginUserCommand>()
                .ReverseMap();
        }
    }
}
