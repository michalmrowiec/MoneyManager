using MoneyManager.Server.Entities;
using MoneyManager.Server.Exceptions;
using MoneyManager.Server.Services;
using MediatR;

namespace MoneyManager.Server.Commands
{
    public class CreateRecordCommandHandler : IRequestHandler<CreateRecordCommand>
    {
        private readonly TrackerDbContext _dbContext;
        private readonly ILogger<CreateRecordCommandHandler> _logger;
        private readonly IUserContextService _userContextService;

        public CreateRecordCommandHandler(TrackerDbContext dbContext, ILogger<CreateRecordCommandHandler> logger, IUserContextService userContextService)
        {
            _dbContext = dbContext ?? throw new InternalServerErrorException(nameof(dbContext));
            _logger = logger ?? throw new InternalServerErrorException(nameof(logger));
            _userContextService = userContextService ?? throw new InternalServerErrorException(nameof(userContextService));
        }

        public Task<Unit> Handle(CreateRecordCommand request, CancellationToken cancellationToken)
        {
            if (request == null) return Task.FromResult(Unit.Value);

            int userId = _userContextService.GetUserId is null ? throw new ForbiddenException() : (int)_userContextService.GetUserId;
            _dbContext.RecordItems.Add(new RecordItem
            {
                Id = request.Id,
                Name = request.Name,
                Amount = request.Amount,
                TransactionDate = request.Date,
                CategoryId = request.CategoryId,
                UserId = userId
            });
            _dbContext.SaveChanges();
            return Task.FromResult(Unit.Value);
        }
    }
}
