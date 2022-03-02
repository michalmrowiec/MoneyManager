﻿using AutoMapper;
using MoneyManager.Application.Functions.Categories.Commands.CreateCategory;
using MoneyManager.Application.Functions.Categories.Commands.UpdateCategory;
using MoneyManager.Application.Functions.Categories.Queries;
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
                .ForMember(m => m.CategoryName, c => c.MapFrom(s => s.Category.CategoryName))
                .ReverseMap();

            CreateMap<Record, UpdateRecordCommand>()
                .ReverseMap();

            CreateMap<LoginUser, LoginUserCommand>()
                .ReverseMap();

            CreateMap<Category, CreateCategoryCommand>()
                .ReverseMap();

            CreateMap<Category, CategoryDto>()
                .ForMember(m => m.Name, c => c.MapFrom(s => s.CategoryName))
                .ReverseMap();

            CreateMap<Category, UpdateCategoryCammand>()
                .ForMember(m => m.Name, c => c.MapFrom(s => s.CategoryName))
                .ReverseMap();
        }
    }
}
