﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MoneyManager.Application.Contracts.Persistence.Users;
using MoneyManager.Domain.Authentication;
using MoneyManager.Domain.Entities;
using MoneyManager.Infractructure.Authentication;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

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
            var expires = DateTime.Now.AddDays(_authenticationSettings.JwtExpireDaysForNormalLogin);

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

            return Task.FromResult(retTok);
        }

        public async Task<bool> CheckEmail(string email)
        {
            return await _dbContext.Users.AnyAsync(x => x.Email == email);
        }

        public async Task<bool> ChangePassword(int userId, string password, string repeatPassword)
        {
            var user = new User
            {
                Id = userId
            };

            if (password == repeatPassword)
            {
                var passwordHash = _passwordHasher.HashPassword(user, password);
                user.PasswordHash = passwordHash;
            }

            _dbContext.Attach(user);
            _dbContext.Entry(user).Property(x => x.PasswordHash).IsModified = true;
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<int?> GetUserId(string userEmail)
        {
            var user = await _dbContext.Users.FirstAsync(x => x.Email == userEmail);
            return user.Id;
        }

        public async Task<bool> ChangeEmail(int userId, string NewEmail)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                return false;

            user.Email = NewEmail;
            await _dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<User> GetUserByIdAsync(int userId)
        {
            var user = await _dbContext.Users
                .Select(u => new User { Id = u.Id, Name = u.Name, Email = u.Email })
                .FirstAsync(u => u.Id == userId);
            return user;
        }
    }
}
