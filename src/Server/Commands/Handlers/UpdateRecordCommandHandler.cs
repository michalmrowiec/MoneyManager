using MoneyManager.Server.Entities;
using MoneyManager.Server.Exceptions;
using MoneyManager.Server.Services;
using MediatR;

namespace MoneyManager.Server.Commands
{
    public class UpdateRecordCommandHandler : IRequestHandler<UpdateRecordCommand>
    {
        private readonly TrackerDbContext _dbContext;
        private readonly ILogger<UpdateRecordCommandHandler> _logger;
        private readonly IUserContextService _userContextService;

        public UpdateRecordCommandHandler(TrackerDbContext dbContext, ILogger<UpdateRecordCommandHandler> logger, IUserContextService userContextService)
        {
            _dbContext = dbContext ?? throw new InternalServerErrorException(nameof(dbContext));
            _logger = logger ?? throw new InternalServerErrorException(nameof(logger));
            _userContextService = userContextService ?? throw new InternalServerErrorException(nameof(userContextService));
        }

        public Task<Unit> Handle(UpdateRecordCommand request, CancellationToken cancellationToken)
        {
            var recordItem = request.RecordItemDto;
            var userId = _userContextService.GetUserId;
            var record = _dbContext.RecordItems.Where(x => x.UserId == userId).FirstOrDefault(x => x.Id == recordItem.Id);
            if (record is null)
            {
                _logger.LogError($"Record with id: {recordItem.Id}, try update by user with id: {userId} - record not found");
                throw new NotFoundException("Record not found");
            }

            record.Name = recordItem.Name;
            record.Amount = recordItem.Amount;
            record.TransactionDate = recordItem.TransactionDate;
            record.CategoryId = recordItem.CategoryId;

            _dbContext.SaveChanges();

            return Task.FromResult(Unit.Value);
        }
    }
}
