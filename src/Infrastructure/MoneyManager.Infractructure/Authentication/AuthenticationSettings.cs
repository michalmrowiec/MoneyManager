namespace MoneyManager.Infractructure.Authentication
{
    public class AuthenticationSettings
    {
        public string JwtKey { get; set; } = null!;
        public int JwtExpireDays { get; set; }
        public string JwtIssuer { get; set; } = null!;
    }
}
