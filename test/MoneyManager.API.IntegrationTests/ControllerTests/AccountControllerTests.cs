using FluentAssertions;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MoneyManager.Application.Functions.Users.Commands.LoginUser;
using MoneyManager.Application.Functions.Users.Commands.RegisterUser;
using MoneyManager.Domain.Authentication;
using MoneyManager.Infractructure;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MoneyManager.API.IntegrationTests.ControllerTests
{
    public class AccountControllerTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly HttpClient _httpClient;
        private readonly string _dbName = Guid.NewGuid().ToString();

        public AccountControllerTests(WebApplicationFactory<Startup> factory)
        {
            _httpClient = factory
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        var dbContextOption = services
                        .SingleOrDefault(service => service.ServiceType == typeof(DbContextOptions<MoneyManagerContext>));

                        if (dbContextOption != null) services.Remove(dbContextOption);

                        services.AddDbContext<MoneyManagerContext>(options => options.UseInMemoryDatabase("TestRecordsDbWithUserToken"));
                    });
                })
                .CreateClient();
        }
        public static IEnumerable<object[]> TestRegisterUsers_ValidModel => new List<object[]>
        {
            new object[] { new RegisterUserCommand { Name = "Test", Email = "test@test.com", Password = "qwerty", RepeatPassword = "qwerty" } },
            new object[] { new RegisterUserCommand { Name = "Test", Email = "test1@test.com", Password = "qwerty", RepeatPassword = "qwerty" } },
            new object[] { new RegisterUserCommand { Name = "Test", Email = "test2@test.com", Password = "qwerty", RepeatPassword = "qwerty" } },
            new object[] { new RegisterUserCommand { Name = "Test", Email = "test3@test.com", Password = "qwerty", RepeatPassword = "qwerty" } },
        };
        public static IEnumerable<object[]> TestRegisterUsers_InvalidModel => new List<object[]>
        {
            new object[] { new RegisterUserCommand { Name = "", Email = "test@test0.com", Password = "qwerty", RepeatPassword = "qwerty" } },
            new object[] { new RegisterUserCommand { Name = "Test", Email = "test1@test0.com", Password = "qwert", RepeatPassword = "qwerty" } },
            new object[] { new RegisterUserCommand { Name = "Test", Email = "test2@test0.com", Password = "qwerty", RepeatPassword = " " } },
            new object[] { new RegisterUserCommand { Name = "Test", Email = "test3est", Password = "qwerty", RepeatPassword = "qwerty" } },
            new object[] { new RegisterUserCommand { Name = "Test above 25 character test", Email = "test4@test0.com", Password = "qwerty", RepeatPassword = "qwerty" } },
        };
        public static IEnumerable<object[]> TestLoginUsers_ValidModel => new List<object[]>
        {
            new object[]
            {
                new RegisterUserCommand { Name = "Test", Email = "test01@test.com", Password = "qwerty", RepeatPassword = "qwerty" },
                new LoginUserCommand { Email = "test01@test.com", Password = "qwerty" }
            },
            new object[]
            {
                new RegisterUserCommand { Name = "Test", Email = "test02@test.com", Password = "qwerty", RepeatPassword = "qwerty" },
                new LoginUserCommand { Email = "test02@test.com", Password = "qwerty" }
            },
            new object[]
            {
                new RegisterUserCommand { Name = "Test", Email = "test03@test.com", Password = "qwerty", RepeatPassword = "qwerty" },
                new LoginUserCommand { Email = "test03@test.com", Password = "qwerty" }
            }
        };
        public static IEnumerable<object[]> TestLoginUsers_InvalidModel => new List<object[]>
        {
            new object[]
            {
                new RegisterUserCommand { Name = "Test", Email = "test01@test3.com", Password = "qwerty", RepeatPassword = "qwerty" },
                new LoginUserCommand { Email = "test01@testERROR3.com", Password = "qwerty" }
            },
            new object[]
            {
                new RegisterUserCommand { Name = "Test", Email = "test02@test3.com", Password = "qwerty", RepeatPassword = "qwerty" },
                new LoginUserCommand { Email = "test02@test3.com", Password = "qwert" }
            },
            new object[]
            {
                new RegisterUserCommand { Name = "Test", Email = "test03@tes3t.com", Password = "qwerty", RepeatPassword = "qwerty" },
                new LoginUserCommand { Email = "test03@test3.com", Password = "qwerty" }
            },
            new object[]
            {
                new RegisterUserCommand { Name = "Test", Email = "test04@tes3t.com", Password = "qwerty", RepeatPassword = "qwerty" },
                new LoginUserCommand { Email = "test", Password = "qwerty" }
            }
        };

        [Theory]
        [MemberData(nameof(TestRegisterUsers_ValidModel))]
        public async Task CreateUser_WithValidModel_ReturnsOkStatus(RegisterUserCommand user)
        {
            var json = JsonConvert.SerializeObject(user);
            var httpContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

            var respond = await _httpClient.PostAsync("/api/account/register/", httpContent);

            respond.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Theory]
        [MemberData(nameof(TestRegisterUsers_InvalidModel))]
        public async Task CreateUser_WithInvalidModel_ReturnsBadRequest(RegisterUserCommand user)
        {
            var json = JsonConvert.SerializeObject(user);
            var httpContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

            var respond = await _httpClient.PostAsync("/api/account/register/", httpContent);

            respond.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Theory]
        [MemberData(nameof(TestLoginUsers_ValidModel))]
        public async Task LoginUser_ForExistUserWithValidModel_ReturnsOkStatusWithUserToken(RegisterUserCommand registerUser, LoginUserCommand loginUser)
        {
            var registerJson = JsonConvert.SerializeObject(registerUser);
            var registerHttpContent = new StringContent(registerJson, UnicodeEncoding.UTF8, "application/json");

            var loginJson = JsonConvert.SerializeObject(loginUser);
            var loginHttpContent = new StringContent(loginJson, UnicodeEncoding.UTF8, "application/json");

            await _httpClient.PostAsync("/api/account/register/", registerHttpContent);

            var loginRespond = await _httpClient.PostAsync("/api/account/login/", loginHttpContent);
            var loginRespondJson = await loginRespond.Content.ReadAsStringAsync();
            var userModel = JsonConvert.DeserializeObject<UserToken>(loginRespondJson)!;

            loginRespond.StatusCode.Should().Be(HttpStatusCode.OK);
            userModel.Email.Should().Be(loginUser.Email);
            userModel.Name.Should().Be(registerUser.Name);
            userModel.Token.Should().NotBeEmpty();
        }

        [Theory]
        [MemberData(nameof(TestLoginUsers_InvalidModel))]
        public async Task LoginUser_ForExistUserWithInvalidModel_ReturnsBadRequest(RegisterUserCommand registerUser, LoginUserCommand loginUser)
        {
            var registerJson = JsonConvert.SerializeObject(registerUser);
            var registerHttpContent = new StringContent(registerJson, UnicodeEncoding.UTF8, "application/json");

            var loginJson = JsonConvert.SerializeObject(loginUser);
            var loginHttpContent = new StringContent(loginJson, UnicodeEncoding.UTF8, "application/json");

            await _httpClient.PostAsync("/api/account/register/", registerHttpContent);

            var loginRespond = await _httpClient.PostAsync("/api/account/login/", loginHttpContent);
            var loginRespondString = await loginRespond.Content.ReadAsStringAsync();

            loginRespond.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            //loginRespondString.Should().Be("Invalid email or password");
        }
    }
}