﻿using MediatR;

namespace MoneyManager.Application.Functions.PlannedBudget.Queries.GetAllYearsWithMonths
{
    public record GetAllYearsWithMonthsQuery(int UserId) : IRequest<Dictionary<int, List<int>>>;
}
