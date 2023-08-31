using System.ComponentModel.DataAnnotations;

namespace MoneyManager.Client.Models.ViewModels
{
    public class FormRecordModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public decimal Amount { get; set; }
        public DateTime TransactionDate { get; set; }
        public int? CategoryId { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? NextDate { get; set; }
        [Range(1, 32, ErrorMessage = "The number must be a day of the month 1-32")]
        public int? RepeatEveryDayOfMonth { get; set; }
    }
}
