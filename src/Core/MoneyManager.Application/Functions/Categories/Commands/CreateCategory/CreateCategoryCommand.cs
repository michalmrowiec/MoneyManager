using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyManager.Application.Functions.Categories.Commands.CreateCategory
{
    public record CreateCategoryCommand : IRequest<CreateCategoryCommandResponse>
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
    }
}
