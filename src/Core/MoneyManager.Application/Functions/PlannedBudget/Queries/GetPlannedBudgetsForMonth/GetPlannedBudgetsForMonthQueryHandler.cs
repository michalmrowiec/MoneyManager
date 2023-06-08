using AutoMapper;
using MediatR;
using MoneyManager.Application.Contracts.Persistence.Items;
using MoneyManager.Application.Functions.Records.Queries.GetRecordsForMonth;

namespace MoneyManager.Application.Functions.PlannedBudget.Queries.GetPlannedBudgetsForMonth
{
    public class GetPlannedBudgetsForMonthQueryHandler : IRequestHandler<GetPlannedBudgetsForMonthQuery, List<PlannedBudgetDto>>
    {
        private readonly IPlannedBudgetRepository _plannedBudgetRepository;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public GetPlannedBudgetsForMonthQueryHandler(IPlannedBudgetRepository plannedBudgetRepository, IMapper mapper, IMediator mediator)
        {
            _plannedBudgetRepository = plannedBudgetRepository;
            _mapper = mapper;
            _mediator = mediator;
        }

        public async Task<List<PlannedBudgetDto>> Handle(GetPlannedBudgetsForMonthQuery request, CancellationToken cancellationToken)
        {
            var plannedBudgets = _mapper.Map<List<PlannedBudgetDto>>
                (await _plannedBudgetRepository.GetRecordsForMonthAsync(request.UserId, request.Year, request.Month));

            if (plannedBudgets.Count == 0)
                return new();

            var records = _mediator.Send(new GetRecordsForMonthQuery(request.UserId, request.Year, request.Month)).Result
                .Where(x => plannedBudgets.Any(q => q.CategoryId == x.CategoryId)).ToList();

            records.ForEach(r => plannedBudgets.First(p => p.CategoryId == r.CategoryId).FilledAmount += r.Amount);

            return plannedBudgets;
        }
    }
}