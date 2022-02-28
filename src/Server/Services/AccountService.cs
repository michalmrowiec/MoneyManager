using MoneyManager.Server.Entities;
using MoneyManager.Server.Exceptions;
using MoneyManager.Shared;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MoneyManager.Server.Services
{
    public interface IAccountService
    {
        void RegisterUser(RegisterUserDto registerUserDto);
        UserToken GenerateJwtToken(LoginUserDto loginUserDto);
    }
    public class AccountService : IAccountService
    {
        private readonly TrackerDbContext _dbContext;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly AuthenticationSettings _authenticationSettings;
        private readonly ILogger<AccountService> _logger;
        public AccountService(TrackerDbContext dbContext, IPasswordHasher<User> passwordHasher, AuthenticationSettings authenticationSettings, ILogger<AccountService> logger)
        {
            _dbContext = dbContext;
            _passwordHasher = passwordHasher;
            _authenticationSettings = authenticationSettings;
            _logger = logger;
            if (!_dbContext.Database.CanConnect())
            {
                _logger.LogError("Database cannot connect");
                throw new InternalServerErrorException("Unable to connect to the database");
            }
        }

        public void RegisterUser(RegisterUserDto registerUserDto)
        {
            var user = _dbContext.Users.FirstOrDefault(x => x.Email == registerUserDto.Email);
            if (user is not null)
            {
                _logger.LogError($"Was tried create the account with same email: {registerUserDto.Email}");
                throw new BadRequestException("Email is already in use");
            }

            var newUser = new User()
            {
                Email = registerUserDto.Email,
                Name = registerUserDto.Name
            };
            if (registerUserDto.Password == registerUserDto.RepeatPassword)
            {
                var passwordHash = _passwordHasher.HashPassword(newUser, registerUserDto.Password);
                newUser.PasswordHash = passwordHash;
            }

            _dbContext.Users.Add(newUser);
            _dbContext.SaveChanges();


            //based categories
            List<CategoryItemDto> baseCategories = new()
            {
                new CategoryItemDto
                {
                    Name = "Food"
                },
                new CategoryItemDto
                {
                    Name="Education"
                }
            };
            var userId = _dbContext.Users.First(x => x.Email == registerUserDto.Email).Id;
            foreach (var item in baseCategories)
            {
                Category category = new()
                {
                    CategoryName = item.Name,
                    UserId = userId
                };
                _dbContext.Categories.Add(category);
            }
            _dbContext.SaveChanges();
        }

        public UserToken GenerateJwtToken(LoginUserDto loginUserDto)
        {
            var user = _dbContext.Users.FirstOrDefault(user => user.Email == loginUserDto.Email);

            if (user is null)
            {
                _logger.LogError($"Was tried login to not existing account with email: {loginUserDto.Email}");
                throw new BadRequestException("Invalid email or password");
            }

            var veryfication = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, loginUserDto.Password);
            if (veryfication == PasswordVerificationResult.Failed)
            {
                _logger.LogError($"Was tried login with wrong password with emial: {loginUserDto.Email}");
                throw new BadRequestException("Invalid email or password");
            }

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Email)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authenticationSettings.JwtKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(_authenticationSettings.JwtExpireDays);

            var token = new JwtSecurityToken(
                _authenticationSettings.JwtIssuer,
                _authenticationSettings.JwtIssuer,
                claims,
                expires: expires,
                signingCredentials: credentials);

            var tokenHandler = new JwtSecurityTokenHandler();

            //return tokenHandler.WriteToken(token);
            return new UserToken
            {
                Token = tokenHandler.WriteToken(token),
                Name = user.Name,
                Email = user.Email
            };
        }
    }
}
