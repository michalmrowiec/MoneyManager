using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyManager.Application.Functions.Categories.Queries
{
    public record CategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
    }
}
