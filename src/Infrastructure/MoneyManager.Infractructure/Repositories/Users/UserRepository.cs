using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MoneyManager.Application.Contracts.Persistence.Users;
using MoneyManager.Domain.Authentication;
using MoneyManager.Domain.Entities;
using MoneyManager.Infractructure.Authentication;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MoneyManager.Infractructure.Repositories.Users
{
    public class UserRepository : IUserAsyncRepository
    {
        protected readonly MoneyManagerContext _dbContext;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly AuthenticationSettings _authenticationSettings;

        public UserRepository(MoneyManagerContext moneyManagerContext, IPasswordHasher<User> passwordHasher, AuthenticationSettings authenticationSettings)
        {
            _dbContext = moneyManagerContext;
            _passwordHasher = passwordHasher;
            _authenticationSettings = authenticationSettings;
        }

        public Task<UserToken> Register(RegisterUser registerUser)
        {
            var user = _dbContext.Users.FirstOrDefault(x => x.Email == registerUser.Email);
            //if (user is not null)
            //{
            //    _logger.LogError($"Was tried create the account with same email: {registerUser.Email}");
            //    throw new BadRequestException("Email is already in use");
            //}

            var newUser = new User()
            {
                Email = registerUser.Email,
                Name = registerUser.Name
            };
            if (registerUser.Password == registerUser.RepeatPassword)
            {
                var passwordHash = _passwordHasher.HashPassword(newUser, registerUser.Password);
                newUser.PasswordHash = passwordHash;
            }

            _dbContext.Users.Add(newUser);
            _dbContext.SaveChanges();
            var login = new LoginUser { Email = registerUser.Email, Password = registerUser.Password };
            return Login(login);


            //based categories
            //List<CategoryItemDto> baseCategories = new()
            //{
            //    new CategoryItemDto
            //    {
            //        Name = "Food"
            //    },
            //    new CategoryItemDto
            //    {
            //        Name = "Education"
            //    }
            //};
            //var userId = _dbContext.Users.First(x => x.Email == registerUser.Email).Id;
            //foreach (var item in baseCategories)
            //{
            //    Category category = new()
            //    {
            //        CategoryName = item.Name,
            //        UserId = userId
            //    };
            //    _dbContext.Categories.Add(category);
            //}
            //_dbContext.SaveChanges();
        }

        public Task<UserToken> Login(LoginUser loginUser)
        {
            var user = _dbContext.Users.FirstOrDefault(user => user.Email == loginUser.Email);

            if (user is null)
            {
                //_logger.LogError($"Was tried login to not existing account with email: {loginUser.Email}");
                //throw new BadRequestException("Invalid email or password");
                return Task.FromResult(new UserToken());
            }

            var veryfication = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, loginUser.Password);
            if (veryfication == PasswordVerificationResult.Failed)
            {
                //_logger.LogError($"Was tried login with wrong password with emial: {loginUser.Email}");
                //throw new BadRequestException("Invalid email or password");
                return Task.FromResult(new UserToken());
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

            var retTok = new UserToken
            {
                Token = tokenHandler.WriteToken(token),
                Name = user.Name,
                Email = user.Email
            };

            //return tokenHandler.WriteToken(token);
            return Task.FromResult(retTok);
        }

        public async Task<bool> CheckEmail(string email)
        {
            return await _dbContext.Users.AnyAsync(x => x.Email == email); 
        }
    }
}
