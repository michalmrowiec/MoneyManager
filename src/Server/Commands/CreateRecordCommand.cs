using MoneyManager.Shared;
using MediatR;

namespace MoneyManager.Server.Commands
{
    public record CreateRecordCommand : IRequest
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? CategoryName { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public int? CategoryId { get; set; }
    }
}
