namespace MoneyManager.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string Name { get; set; }

        public List<Record> Records { get; set; } = new List<Record>();
        public List<Category> Categories { get; set; } = new List<Category>();
        public List<RecurringRecord> RecurringRecords { get; set; } = new List<RecurringRecord>();
        public List<PlannedBudget> PlannedBudgets { get; set; } = new List<PlannedBudget>();
    }
}
