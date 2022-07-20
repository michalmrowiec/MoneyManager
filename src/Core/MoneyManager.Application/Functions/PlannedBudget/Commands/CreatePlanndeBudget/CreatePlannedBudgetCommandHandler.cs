using AutoMapper;
using MediatR;
using MoneyManager.Application.Contracts.Persistence.Items;
using MoneyManager.Application.Functions.PlannedBudget.Commands.CreatePlanndeBudgetRecord;

namespace MoneyManager.Application.Functions.PlannedBudget.Commands.CreatePlanndeBudget
{
    public class CreatePlannedBudgetCommandHandler : IRequestHandler<CreatePlannedBudgetCommand, CreatePlannedBudgetCommandResponse>
    {
        private readonly IPlannedBudgetRepository _plannedBudgetRepository;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public CreatePlannedBudgetCommandHandler(IPlannedBudgetRepository plannedBudgetRepository, IMapper mapper, IMediator mediator)
        {
            _plannedBudgetRepository = plannedBudgetRepository;
            _mapper = mapper;
            _mediator = mediator;
        }
        public async Task<CreatePlannedBudgetCommandResponse> Handle(CreatePlannedBudgetCommand request, CancellationToken cancellationToken)
        {
            var validator = new CreatePlannedBudgetCommandValidator(_mediator);
            var validatorResult = await validator.ValidateAsync(request);

            if (!validatorResult.IsValid)
                return new CreatePlannedBudgetCommandResponse(validatorResult);

            var planndedBudget = _mapper.Map<Domain.Entities.PlannedBudget>(request);

            planndedBudget = await _plannedBudgetRepository.AddAsync(planndedBudget);

            return new CreatePlannedBudgetCommandResponse(planndedBudget.Id);
        }
    }
}
