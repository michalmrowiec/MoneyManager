using AutoMapper;
using MediatR;
using MoneyManager.Application.Contracts.Persistence.Items;

namespace MoneyManager.Application.Functions.PlannedBudget.Queries.GetPlannedBudgetsForMonth
{
    public class GetPlannedBudgetsForMonthQueryHandler : IRequestHandler<GetPlannedBudgetsForMonthQuery, List<PlannedBudgetDto>>
    {
        private readonly IPlannedBudgetRepository _plannedBudgetRepository;
        private readonly IMapper _mapper;

        public GetPlannedBudgetsForMonthQueryHandler(IPlannedBudgetRepository plannedBudgetRepository, IMapper mapper, IMediator mediator)
        {
            _plannedBudgetRepository = plannedBudgetRepository;
            _mapper = mapper;
        }

        public async Task<List<PlannedBudgetDto>> Handle(GetPlannedBudgetsForMonthQuery request, CancellationToken cancellationToken)
        {
            return _mapper.Map<List<PlannedBudgetDto>>
                (await _plannedBudgetRepository.GetPlannedBudgetsForMonth(request.UserId, request.Year, request.Month));
        }
    }
}