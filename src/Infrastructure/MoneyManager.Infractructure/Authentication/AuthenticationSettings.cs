namespace MoneyManager.Infractructure.Authentication
{
    public class AuthenticationSettings
    {
        public string JwtKey { get; set; } = null!;
        public int JwtExpireDaysForNormalLogin { get; set; }
        public int JwtExpireHoursForResetPassword { get; set; }
        public string JwtIssuer { get; set; } = null!;
    }
}
