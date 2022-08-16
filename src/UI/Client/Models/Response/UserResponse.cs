
namespace MoneyManager.Client.Models.Response
{
    public class UserResponse : BaseResponse
    {
        public UserToken? UserToken { get; set; }
    }

    public class UserToken
    {
        public string Token { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
    }
}
