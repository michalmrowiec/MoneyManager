namespace MoneyManager.Server.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public string CategoryName { get; set; } = null!;

        public int UserId { get; set; }
        public virtual User? User { get; set; }
    }
}
