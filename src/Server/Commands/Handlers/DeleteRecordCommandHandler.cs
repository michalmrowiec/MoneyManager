using MoneyManager.Server.Entities;
using MoneyManager.Server.Exceptions;
using MoneyManager.Server.Services;
using MediatR;

namespace MoneyManager.Server.Commands
{
    public class DeleteRecordCommandHandler : IRequestHandler<DeleteRecordCommand>
    {
        private readonly TrackerDbContext _dbContext;
        private readonly ILogger<DeleteRecordCommandHandler> _logger;
        private readonly IUserContextService _userContextService;

        public DeleteRecordCommandHandler(TrackerDbContext dbContext, ILogger<DeleteRecordCommandHandler> logger, IUserContextService userContextService)
        {
            _dbContext = dbContext ?? throw new InternalServerErrorException(nameof(dbContext));
            _logger = logger ?? throw new InternalServerErrorException(nameof(logger));
            _userContextService = userContextService ?? throw new InternalServerErrorException(nameof(userContextService));
        }

        public Task<Unit> Handle(DeleteRecordCommand request, CancellationToken cancellationToken)
        {
            var id = request.Id;
            var userId = _userContextService.GetUserId;
            var record = _dbContext.RecordItems.Where(x => x.UserId == userId).FirstOrDefault(x => x.Id == id);
            if (record is null)
            {
                _logger.LogError($"Record with id: {id}, try delete by user with id: {userId} - record not found");
                throw new NotFoundException("Record not found");
            }

            _dbContext.RecordItems.Remove(record);
            _dbContext.SaveChanges();

            return Task.FromResult(Unit.Value);
        }
    }
}
