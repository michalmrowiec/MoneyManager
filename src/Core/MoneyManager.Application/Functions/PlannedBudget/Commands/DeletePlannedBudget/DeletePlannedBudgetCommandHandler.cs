using MediatR;
using MoneyManager.Application.Contracts.Persistence.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyManager.Application.Functions.PlannedBudget.Commands.DeletePlannedBudget
{
    public class DeletePlannedBudgetCommandHandler : IRequestHandler<DeletePlannedBudgetCommand>
    {
        private readonly IPlannedBudgetRepository _plannedBudgetRepository;

        public DeletePlannedBudgetCommandHandler(IPlannedBudgetRepository plannedBudgetRepository)
        {
            _plannedBudgetRepository = plannedBudgetRepository;
        }
        public async Task<Unit> Handle(DeletePlannedBudgetCommand request, CancellationToken cancellationToken)
        {
            await _plannedBudgetRepository
                .DeleteAsync(request.UserId,await _plannedBudgetRepository.GetByIdAsync(request.UserId, request.PlannedBudgetId));
            return Unit.Value;
        }
    }
}
