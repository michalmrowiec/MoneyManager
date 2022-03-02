using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyManager.Application.Functions.Categories.Commands.UpdateCategory
{
    public record UpdateCategoryCammand : IRequest
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int UserId { get; set; }

    }
}
