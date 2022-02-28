using System.ComponentModel.DataAnnotations;

namespace MoneyManager.Shared
{
    public class RecordItemDto : IRecord
    {
        public int Id { get; set; }
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
