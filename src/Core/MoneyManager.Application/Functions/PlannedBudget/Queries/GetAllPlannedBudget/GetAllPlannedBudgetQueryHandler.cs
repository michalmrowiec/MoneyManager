using AutoMapper;
using MediatR;
using MoneyManager.Application.Contracts.Persistence.Items;

namespace MoneyManager.Application.Functions.PlannedBudget.Queries.GetAllPlannedBudget
{
    public class GetAllPlannedBudgetQueryHandler : IRequestHandler<GetAllPlannedBudgetQuery, List<PlannedBudgetDto>>
    {
        private readonly IPlannedBudgetRepository _plannedBudgetRepository;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public GetAllPlannedBudgetQueryHandler(IPlannedBudgetRepository plannedBudgetRepository, IMapper mapper, IMediator mediator)
        {
            _plannedBudgetRepository = plannedBudgetRepository;
            _mapper = mapper;
            _mediator = mediator;
        }

        public async Task<List<PlannedBudgetDto>> Handle(GetAllPlannedBudgetQuery request, CancellationToken cancellationToken)
        {
            return _mapper.Map<List<PlannedBudgetDto>>(await _plannedBudgetRepository.GetAllRecordsAsync(request.UserId));
        }
    }
}