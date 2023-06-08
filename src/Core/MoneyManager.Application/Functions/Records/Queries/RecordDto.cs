using MoneyManager.Domain.Entities;

namespace MoneyManager.Application.Functions.Records
{
    public record RecordDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        //public string? CategoryName { get; set; }
        public decimal Amount { get; set; }
        public DateTime TransactionDate { get; set; }
        public int? CategoryId { get; set; }
        public Category? Category { get; set; }
    }
}
