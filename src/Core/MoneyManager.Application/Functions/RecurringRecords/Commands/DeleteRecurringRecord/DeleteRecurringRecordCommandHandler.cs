using MediatR;
using MoneyManager.Application.Contracts.Persistence.Items;

namespace MoneyManager.Application.Functions.RecurringRecords.Commands.DeleteRecurringRecord
{
    public class DeleteRecurringRecordCommandHandler : IRequestHandler<DeleteRecurringRecordCommand>
    {
        private readonly IRecurringRecordRepository _recurringRecordRepsitory;

        public DeleteRecurringRecordCommandHandler(IRecurringRecordRepository recurringRecordRepsitory)
        {
            _recurringRecordRepsitory = recurringRecordRepsitory;
        }

        public async Task<Unit> Handle(DeleteRecurringRecordCommand request, CancellationToken cancellationToken)
        {
            var recurringRecordToDelete = await _recurringRecordRepsitory.GetByIdAsync(request.UserId, request.Id);
            await _recurringRecordRepsitory.DeleteAsync(request.UserId, recurringRecordToDelete);
            return Unit.Value;
        }
    }
}
