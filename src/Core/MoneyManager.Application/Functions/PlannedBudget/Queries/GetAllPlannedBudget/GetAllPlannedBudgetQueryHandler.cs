using AutoMapper;
using MediatR;
using MoneyManager.Application.Contracts.Persistence.Items;

namespace MoneyManager.Application.Functions.PlannedBudget.Queries.GetAllPlannedBudget
{
    public class GetAllPlannedBudgetQueryHandler : IRequestHandler<GetAllPlannedBudgetQuery, List<PlannedBudgetDto>>
    {
        private readonly IPlannedBudgetRepository _plannedBudgetRepository;
        private readonly IMapper _mapper;

        public GetAllPlannedBudgetQueryHandler(IPlannedBudgetRepository plannedBudgetRepository, IMapper mapper)
        {
            _plannedBudgetRepository = plannedBudgetRepository;
            _mapper = mapper;
        }

        public async Task<List<PlannedBudgetDto>> Handle(GetAllPlannedBudgetQuery request, CancellationToken cancellationToken)
        {
            return _mapper.Map<List<PlannedBudgetDto>>(await _plannedBudgetRepository.GetAllAsync(request.UserId));
        }
    }
}