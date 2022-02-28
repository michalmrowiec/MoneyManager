namespace MoneyManager.Server.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public string Name { get; set; } = null!;

        //public virtual List<RecordItem>? Records { get; set; }
        //public virtual List<Category>? Categories { get; set; }
    }
}
