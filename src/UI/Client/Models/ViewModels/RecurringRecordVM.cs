using System.ComponentModel.DataAnnotations;

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
