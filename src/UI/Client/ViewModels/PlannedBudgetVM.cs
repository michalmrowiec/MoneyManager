using MoneyManager.Client.ViewModels.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyManager.Client.ViewModels
{
    public class PlannedBudgetVM : IRecord, IRecordWithDate
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        /// <summary>
        /// Plan For Month
        /// </summary>
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public decimal FilledAmount { get; set; }
        public int CategoryId { get; set; }
        /// <summary>
        /// Category Name
        /// </summary>
    }
}
