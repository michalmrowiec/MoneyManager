using AutoMapper;
using MediatR;
using MoneyManager.Application.Contracts.Persistence.Items;

namespace MoneyManager.Application.Functions.PlannedBudget.Queries.GetPlannedBudgetById
{
    public class GetPlannedBudgetByIdQueryHandler : IRequestHandler<GetPlannedBudgetByIdQuery, PlannedBudgetDto>
    {
        private readonly IPlannedBudgetRepository _plannedBudgetRepository;
        private readonly IMapper _mapper;
        public GetPlannedBudgetByIdQueryHandler(IPlannedBudgetRepository plannedBudgetRepository, IMapper mapper)
        {
            _plannedBudgetRepository = plannedBudgetRepository;
            _mapper = mapper;
        }

        public async Task<PlannedBudgetDto> Handle(GetPlannedBudgetByIdQuery request, CancellationToken cancellationToken)
        {
            return _mapper.Map<PlannedBudgetDto>(await _plannedBudgetRepository.GetByIdAsync(request.UserId, request.PlannedBudgetId));
        }
    }
}