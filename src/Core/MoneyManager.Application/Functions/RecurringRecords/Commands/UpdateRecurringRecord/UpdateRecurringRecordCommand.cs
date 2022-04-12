using MediatR;
using MoneyManager.Application.Functions.Records;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyManager.Application.Functions.RecurringRecords.Commands.UpdateRecurringRecord
{
    public record UpdateRecurringRecordCommand : IRequest
    {
		public int Id { get; set; }
		//public CreateRecordCommand Record { get; set; } = null!;
		public bool IsActive { get; set; }

		public DateTime NextDate { get; set; }

		//public int? RepeatAfterDays { get; set; }
		public int RepeatEveryDayOfMonth { get; set; }

		//public DateTime? EndDate { get; set; }
		//public int? AmountOfTimes { get; set; }

		public string Name { get; set; } = null!;
		public decimal Amount { get; set; }
		public DateTime? TransactionDate { get; set; }
		public int? CategoryId { get; set; }
		public int UserId { get; set; }
	}
}
