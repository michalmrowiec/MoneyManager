using MoneyManager.Domain.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyManager.Domain.Entities
{
    public class PlannedBudget : IIdentifier
    {
        public int Id { get; set; } 
        public DateTime PlanForMonth { get; set; }
        public decimal Amount { get; set; }
        public decimal FilledAmount { get; set; }

        public int? CategoryId { get; set; }
        public int UserId { get; set; }


        public virtual User? User { get; set; }
        public virtual Category? Category { get; set; }
    }
}
