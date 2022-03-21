using MediatR;

namespace MoneyManager.Application.Functions.Records
{
    public record UpdateRecordCommand : IRequest
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? CategoryName { get; set; }
        public decimal Amount { get; set; }
        public DateTime TransactionDate { get; set; }
        public int? CategoryId { get; set; }
        public int UserId { get; set; }
    }
}
