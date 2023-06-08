using MediatR;
using MoneyManager.Application.Functions.Records.Commands.CreateRecord;

namespace MoneyManager.Application.Functions.Records
{
    public record CreateRecordCommand : IRequest<CreateRecordCommandResponse>
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        //HACK to delete
        //public string? CategoryName { get; set; }
        public decimal Amount { get; set; }
        public DateTime TransactionDate { get; set; }
        public int CategoryId { get; set; }
        public int UserId { get; set; }
    }
}
