using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyManager.Client.ViewModels
{
    public class RecurringRecordVM
    {
        public int Id { get; set; }
        public bool IsActive { get; set; }
        [Required]
        public DateTime NextDate { get; set; }
        [Required]
        [Range(1, 32, ErrorMessage = "The number must be a day of the month 1-32")]
        public int RepeatEveryDayOfMonth { get; set; }

        [Required]
        [StringLength(25, ErrorMessage = "Name is too long.")]
        public string Name { get; set; } = null!;
        public string? CategoryName { get; set; }
        [Required]
        public decimal Amount { get; set; }
        public DateTime TransactionDate { get; set; }
        public int? CategoryId { get; set; }
    }
}
