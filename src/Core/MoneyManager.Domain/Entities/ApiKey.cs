namespace MoneyManager.Domain.Entities
{
    public class ApiKey
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public string ClientName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ExpiresAt { get; set; }
        public bool Active { get; set; }
        public string Permissions { get; set;}
    }
}
