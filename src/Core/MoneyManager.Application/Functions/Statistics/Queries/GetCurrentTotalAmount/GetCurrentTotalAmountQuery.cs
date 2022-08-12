using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyManager.Application.Functions.Statistics.Queries.GetCurrentTotalAmount
{
    public record GetCurrentTotalAmountQuery(int UserId) : IRequest<decimal>;
}
