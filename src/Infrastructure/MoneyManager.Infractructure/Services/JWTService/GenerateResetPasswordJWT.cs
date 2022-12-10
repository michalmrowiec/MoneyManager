using Microsoft.IdentityModel.Tokens;
using MoneyManager.Application.Contracts.Persistence;
using MoneyManager.Domain.Authentication;
using MoneyManager.Infractructure.Authentication;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MoneyManager.Infractructure.Services.JWTService
{
    internal class GenerateResetPasswordJWT : IGenerateResetPasswordJWT
    {
        private readonly AuthenticationSettings _authenticationSettings;

        public GenerateResetPasswordJWT(AuthenticationSettings authenticationSettings)
        {
            _authenticationSettings = authenticationSettings;
        }

        public UserToken GenerateToken(string userEmail, int userId, string userName)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(ClaimTypes.Name, userName),
                new Claim(ClaimTypes.Email, userEmail)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authenticationSettings.JwtKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddHours(_authenticationSettings.JwtExpireHoursForResetPassword);

            var token = new JwtSecurityToken(
                _authenticationSettings.JwtIssuer,
                _authenticationSettings.JwtIssuer,
                claims,
                expires: expires,
                signingCredentials: credentials);

            var tokenHandler = new JwtSecurityTokenHandler();

            var userToken = new UserToken
            {
                Token = tokenHandler.WriteToken(token),
                Name = userName,
                Email = userEmail
            };

            return userToken;
        }
    }
}
