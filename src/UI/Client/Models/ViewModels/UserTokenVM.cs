namespace MoneyManager.Client.ViewModels
{
    public class UserTokenVM
    {
        public string Token { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public int UserId { get; set; }
    }
}
