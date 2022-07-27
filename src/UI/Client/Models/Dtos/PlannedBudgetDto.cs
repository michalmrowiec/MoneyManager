using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyManager.Client.ViewModels.Dtos
{
    public class PlannedBudgetDto
    {
        public int Id { get; set; }
        public DateTime PlanForMonth { get; set; }
        public decimal Amount { get; set; }
        public decimal FilledAmount { get; set; }

        public int? CategoryId { get; set; }
        public int UserId { get; set; }
    }
}
