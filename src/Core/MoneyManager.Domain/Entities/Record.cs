using MoneyManager.Domain.Entities.Interfaces;

namespace MoneyManager.Domain.Entities
{
    public class Record : IIdentifier, IUserIdentiifier
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public DateTime TransactionDate { get; set; }

        public Category Category { get; set; }
        public int CategoryId { get; set; }
        public User User { get; set; }
        public int UserId { get; set; }
    }
}
