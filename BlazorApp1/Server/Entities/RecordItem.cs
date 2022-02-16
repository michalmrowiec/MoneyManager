namespace BlazorApp1.Server.Entities
{
    public class RecordItem
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public decimal Amount { get; set; }
        public DateTime TransactionDate { get; set; }

        public int? CategoryId { get; set; }
        public virtual Category? Category { get; set; }
        public int UserId { get; set; }
        public virtual User? User { get; set; }
    }
}
