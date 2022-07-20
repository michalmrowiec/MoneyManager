﻿using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyManager.Application.Functions.PlannedBudget.Queries.GetAllPlannedBudget
{
    public record GetAllPlannedBudgetQuery(int UserId) : IRequest<List<PlannedBudgetDto>>;
}
