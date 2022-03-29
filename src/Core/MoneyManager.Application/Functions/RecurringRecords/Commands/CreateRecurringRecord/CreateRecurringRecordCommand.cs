using MediatR;
using MoneyManager.Application.Functions.Records;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyManager.Application.Functions.RecurringRecords.Commands.CreateRecurringRecord
{
    public record CreateRecurringRecordCommand : IRequest<CreateRecurringRecordCommandResponse>
    {
		public int Id { get; set; }
		public CreateRecordCommand Record { get; set; } = null!;
		public bool IsActive { get; set; }

		public DateTime NextDate { get; set; }

		//public int? RepeatAfterDays { get; set; }
		public int RepeatEveryDayOfMonth { get; set; }

		//public DateTime? EndDate { get; set; }
		//public int? AmountOfTimes { get; set; }

		public int UserId { get; set; }
	}
}
