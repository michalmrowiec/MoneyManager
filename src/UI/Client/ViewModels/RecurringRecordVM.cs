using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyManager.Client.ViewModels
{
    public class RecurringRecordVM : RecordVM
    {
        public bool IsActive { get; set; }
        [Required]
        public DateTime NextDate { get; set; }
        [Required]
        [Range(1, 32, ErrorMessage = "The number must be a day of the month 1-32")]
        public int RepeatEveryDayOfMonth { get; set; }
    }
}
