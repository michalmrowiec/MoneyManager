using AutoMapper;
using MediatR;
using MoneyManager.Application.Contracts.Persistence.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyManager.Application.Functions.PlannedBudget.Commands.UpdatePlannedBudget
{
    public class UpdatePlannedBudgetCommandHandler : IRequestHandler<UpdatePlannedBudgetCommand>
    {
        private readonly IPlannedBudgetRepository _plannedBudgetRepository;
        private readonly IMapper _mapper;

        public UpdatePlannedBudgetCommandHandler(IPlannedBudgetRepository plannedBudgetRepository, IMapper mapper)
        {
            _plannedBudgetRepository = plannedBudgetRepository;
            _mapper = mapper;
        }
        public async Task<Unit> Handle(UpdatePlannedBudgetCommand request, CancellationToken cancellationToken)
        {
            var plannedBudget = _mapper.Map<Domain.Entities.PlannedBudget>(request);

            await _plannedBudgetRepository.UpdateAsync(plannedBudget);
            
            return Unit.Value;
        }
    }
}
