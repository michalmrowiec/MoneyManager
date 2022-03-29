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
		public Record Record { get; set; } = null!;
		public bool IsActive { get; set; }

		public DateTime NextDate { get; set; }

		//public int? RepeatAfterDays { get; set; }
		public int RepeatEveryDayOfMonth { get; set; }

		//public DateTime? EndDate { get; set; }
		//public int? AmountOfTimes { get; set; }

		public int UserId { get; set; }
		public virtual User? User { get; set; }
	}
}
