using AutoMapper;
using MoneyManager.Application.Functions.Categories.Commands.CreateCategory;
using MoneyManager.Application.Functions.Categories.Commands.UpdateCategory;
using MoneyManager.Application.Functions.Categories.Queries;
using MoneyManager.Application.Functions.CryptoAssets.Commands.CreateCryptoAsset;
using MoneyManager.Application.Functions.CryptoAssets.Commands.UpdateCryptoAsset;
using MoneyManager.Application.Functions.PlannedBudget.Commands.CreatePlanndeBudgetRecord;
using MoneyManager.Application.Functions.PlannedBudget.Commands.UpdatePlannedBudget;
using MoneyManager.Application.Functions.PlannedBudget.Queries;
using MoneyManager.Application.Functions.Records;
using MoneyManager.Application.Functions.RecurringRecords.Commands.CreateRecurringRecord;
using MoneyManager.Application.Functions.RecurringRecords.Commands.UpdateRecurringRecord;
using MoneyManager.Application.Functions.RecurringRecords.Queries;
using MoneyManager.Application.Functions.Users.Commands.LoginUser;
using MoneyManager.Application.Functions.Users.Commands.RegisterUser;
using MoneyManager.Domain.Authentication;
using MoneyManager.Domain.Entities;
using MoneyManager.Domain.Entities.CryptoAssets;

namespace MoneyManager.Application.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Record, CreateRecordCommand>()
                .ForMember(m => m.CategoryId, c => c.MapFrom(s => s.CategoryId))
                .ReverseMap();

            CreateMap<Record, RecordDto>()
                .ReverseMap();

            CreateMap<Record, UpdateRecordCommand>()
                .ReverseMap();

            CreateMap<LoginUser, LoginUserCommand>()
                .ReverseMap();

            CreateMap<Category, CreateCategoryCommand>()
                .ReverseMap();

            CreateMap<Category, CategoryDto>()
                .ReverseMap();

            CreateMap<Category, UpdateCategoryCammand>()
                .ReverseMap();

            CreateMap<RegisterUser, RegisterUserCommand>()
                .ReverseMap();

            CreateMap<LoginUser, LoginUserCommand>()
                .ReverseMap();

            CreateMap<RecurringRecord, CreateRecurringRecordCommand>()
                .ReverseMap();

            CreateMap<RecurringRecord, RecurringRecordDto>()
                .ReverseMap();

            CreateMap<RecurringRecord, UpdateRecurringRecordCommand>()
                .ReverseMap();

            CreateMap<PlannedBudget, CreatePlannedBudgetCommand>()
                .ReverseMap();

            CreateMap<PlannedBudget, UpdatePlannedBudgetCommand>()
                .ReverseMap();

            CreateMap<PlannedBudget, PlannedBudgetDto>()
                .ReverseMap();

            CreateMap<CryptoAsset, CreateCryptoAssetCommand>()
                .ReverseMap();

            CreateMap<CryptoAsset, UpdateCryptoAssetCommand>()
                .ReverseMap();
        }
    }
}
