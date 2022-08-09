using MoneyManager.Client.Models.ViewModels.Interfaces;
using MoneyManager.Client.ViewModels.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyManager.Client.ViewModels
{
    public class PlannedBudgetVM : IRecord, IId, IRecordWithDate
    {
        public int Id { get; set; }
        /// <summary>
        /// Category Name
        /// </summary>
        public string Name { get; set; } = null!;
        /// <summary>
        /// Plan For Month
        /// </summary>
        public DateTime TransactionDate { get; set; }
        public decimal Amount { get; set; }
        public decimal FilledAmount { get; set; }
        public int? CategoryId { get; set; }

    }
}
