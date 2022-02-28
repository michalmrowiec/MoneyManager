using MoneyManager.Server.Entities;
using MoneyManager.Server.Exceptions;
using MoneyManager.Server.Services;
using MoneyManager.Shared;
using MediatR;

namespace MoneyManager.Server.Queries
{
    public class GetAllRecordsQueryHandler : IRequestHandler<GetAllRecordsQuery, List<RecordItemDto>>
    {
        private readonly TrackerDbContext _dbContext;
        private readonly ILogger<GetAllRecordsQueryHandler> _logger;
        private readonly IUserContextService _userContextService;

        public GetAllRecordsQueryHandler(TrackerDbContext dbContext, ILogger<GetAllRecordsQueryHandler> logger, IUserContextService userContextService)
        {
            _dbContext = dbContext ?? throw new InternalServerErrorException(nameof(dbContext));
            _logger = logger ?? throw new InternalServerErrorException(nameof(logger));
            _userContextService = userContextService ?? throw new InternalServerErrorException(nameof(userContextService));
        }

        public Task<List<RecordItemDto>> Handle(GetAllRecordsQuery request, CancellationToken cancellationToken)
        {
            var userId = _userContextService.GetUserId;
            var records = _dbContext.RecordItems.Where(x => x.UserId == userId).ToList();
            var categories = _dbContext.Categories.Where(x => x.UserId == userId).ToList();

            if (records is null)
            {
                _logger.LogError($"User records with id: {userId} were not found");
                throw new NotFoundException("Records not found");
            }

            // LEFT OUTER JOIN -> join category to records
            var listOfrecords =
                from record in records
                join category in categories on record.CategoryId equals category.Id into ps
                from supCategory in ps.DefaultIfEmpty()
                select new RecordItemDto
                {
                    Id = record.Id,
                    Name = record.Name,
                    Amount = record.Amount,
                    TransactionDate = record.TransactionDate,
                    CategoryId = supCategory?.Id,
                    CategoryName = supCategory == null ? null : supCategory.CategoryName ?? ""
                };

            //return listOfrecords.ToList();
            return Task.FromResult(listOfrecords.ToList());
        }
    }
}
