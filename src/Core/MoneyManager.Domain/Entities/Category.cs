using MoneyManager.Domain.Entities.Interfaces;

namespace MoneyManager.Domain.Entities
{
    public class Category : IIdentifier
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public User User { get; set; }
        public int UserId { get; set; }
    }
}
