using BlazorApp1.Server.Entities;
using BlazorApp1.Server.Exceptions;
using BlazorApp1.Shared;

namespace BlazorApp1.Server.Services
{
    public interface ITrackerService
    {
        public void Post(RecordItemDto recordItem);
        public List<RecordItemDto> GetAllRecords();
        public void Delete(int id);
        public void Update(RecordItemDto recordItem);
        public List<RecordItemDto> GetRecordsForCategory(int categoryId);
    }

    public class TrackerService : ITrackerService
    {
        private readonly TrackerDbContext _dbContext;
        private readonly ILogger<TrackerService> _logger;
        private readonly IUserContextService _userContextService;

        public TrackerService(TrackerDbContext dbContext, ILogger<TrackerService> logger, IUserContextService userContextService)
        {
            _dbContext = dbContext;
            _logger = logger;
            _userContextService = userContextService;

            if (!_dbContext.Database.CanConnect())
            {
                _logger.LogError("Database cannot connect");
                throw new InternalServerErrorException("Unable to connect to the database");
            }
        }

        public void Delete(int id)
        {
            var userId = _userContextService.GetUserId;
            var record = _dbContext.RecordItems.Where(x => x.UserId == userId).FirstOrDefault(x => x.Id == id);
            if (record is null)
            {
                _logger.LogError($"Record with id: {id}, try delete by user with id: {userId} - record not found");
                throw new NotFoundException("Record not found");
            }

            _dbContext.RecordItems.Remove(record);
            _dbContext.SaveChanges();
        }

        public void Update(RecordItemDto recordItem)
        {
            var userId = _userContextService.GetUserId;
            var record = _dbContext.RecordItems.Where(x => x.UserId == userId).FirstOrDefault(x => x.Id == recordItem.Id);
            if (record is null)
            {
                _logger.LogError($"Record with id: {recordItem.Id}, try update by user with id: {userId} - record not found");
                throw new NotFoundException("Record not found");
            }

            record.Name = recordItem.Name;
            record.Amount = recordItem.Amount;
            record.TransactionDate = recordItem.Date;
            record.CategoryId = recordItem.CategoryId;

            _dbContext.SaveChanges();
        }

        public List<RecordItemDto> GetAllRecords()
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
                    Date = record.TransactionDate,
                    CategoryId = supCategory?.Id,
                    CategoryName = supCategory == null ? null : supCategory.CategoryName ?? ""
                };

            return listOfrecords.ToList();
        }

        public void Post(RecordItemDto recordItem)
        {
            int userId = _userContextService.GetUserId is null ? throw new ForbiddenException() : (int)_userContextService.GetUserId;

            _dbContext.RecordItems.Add(new RecordItem
            {
                Id = recordItem.Id,
                Name = recordItem.Name,
                Amount = recordItem.Amount,
                TransactionDate = recordItem.Date,
                CategoryId = recordItem.CategoryId,
                UserId = userId
            });
            _dbContext.SaveChanges();
        }

        public List<RecordItemDto> GetRecordsForCategory(int categoryId)
        {
            var userId = _userContextService.GetUserId;
            var recordsForCategory = _dbContext.RecordItems.Where(x => x.UserId == userId && x.CategoryId == categoryId).ToList();
            var category = _dbContext.Categories.Where(x => x.UserId == userId).First(x => x.Id == categoryId);

            var listOfRecords = new List<RecordItemDto>();

            if (recordsForCategory is null || category is null) return listOfRecords;

            foreach (var recordItem in recordsForCategory)
            {
                listOfRecords.Add(new RecordItemDto
                {
                    Id = recordItem.Id,
                    Name = recordItem.Name,
                    Amount = recordItem.Amount,
                    Date = recordItem.TransactionDate,
                    CategoryId = category.Id,
                    CategoryName = category.CategoryName
                });
            }
            return listOfRecords;
        }
    }
}
