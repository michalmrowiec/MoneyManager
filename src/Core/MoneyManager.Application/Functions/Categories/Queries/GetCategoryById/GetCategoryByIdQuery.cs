using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyManager.Application.Functions.Categories.Queries.GetCategoryById
{
    public record GetCategoryByIdQuery(int UserId, int CategoryId) : IRequest<CategoryDto>;
}
