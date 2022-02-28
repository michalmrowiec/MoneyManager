using MoneyManager.Shared;
using MediatR;

namespace MoneyManager.Application.Functions.Records
{
    public record UpdateRecordCommand() : IRequest
    {
        public string Name { get; set; } = null!;
        public string? CategoryName { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public int? CategoryId { get; set; }
        public int UserId { get; set; }
    }
}
