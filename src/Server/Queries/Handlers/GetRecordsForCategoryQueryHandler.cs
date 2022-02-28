using MoneyManager.Server.Entities;
using MoneyManager.Server.Exceptions;
using MoneyManager.Server.Services;
using MoneyManager.Shared;
using MediatR;

namespace MoneyManager.Server.Queries
{
    public class GetRecordsForCategoryQueryHandler : IRequestHandler<GetRecordsForCategoryQuery, List<RecordItemDto>>
    {
        private readonly TrackerDbContext _dbContext;
        private readonly ILogger<GetRecordsForCategoryQueryHandler> _logger;
        private readonly IUserContextService _userContextService;

        public GetRecordsForCategoryQueryHandler(TrackerDbContext dbContext, ILogger<GetRecordsForCategoryQueryHandler> logger, IUserContextService userContextService)
        {
            _dbContext = dbContext ?? throw new InternalServerErrorException(nameof(dbContext));
            _logger = logger ?? throw new InternalServerErrorException(nameof(logger));
            _userContextService = userContextService ?? throw new InternalServerErrorException(nameof(userContextService));
        }

        public Task<List<RecordItemDto>> Handle(GetRecordsForCategoryQuery request, CancellationToken cancellationToken)
        {
            var categoryId = request.Id;
            var userId = _userContextService.GetUserId;
            var recordsForCategory = _dbContext.RecordItems.Where(x => x.UserId == userId && x.CategoryId == categoryId).ToList();
            var category = _dbContext.Categories.Where(x => x.UserId == userId).First(x => x.Id == categoryId);

            var listOfRecords = new List<RecordItemDto>();

            if (recordsForCategory is null || category is null) return Task.FromResult(listOfRecords);

            foreach (var recordItem in recordsForCategory)
            {
                listOfRecords.Add(new RecordItemDto
                {
                    Id = recordItem.Id,
                    Name = recordItem.Name,
                    Amount = recordItem.Amount,
                    TransactionDate = recordItem.TransactionDate,
                    CategoryId = category.Id,
                    CategoryName = category.CategoryName
                });
            }
            return Task.FromResult(listOfRecords);
        }
    }
}
