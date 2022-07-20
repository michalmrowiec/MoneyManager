using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyManager.Domain.Entities
{
    public class RecurringRecord
    {
		public int Id { get; set; }
		public bool IsActive { get; set; }
		public DateTime NextDate { get; set; }
		public int RepeatEveryDayOfMonth { get; set; }

		public string Name { get; set; } = null!;
		public decimal Amount { get; set; }
		public DateTime? TransactionDate { get; set; }
		public int? CategoryId { get; set; }
		public virtual Category? Category { get; set; }

		public int UserId { get; set; }
		public virtual User? User { get; set; }
	}
}
