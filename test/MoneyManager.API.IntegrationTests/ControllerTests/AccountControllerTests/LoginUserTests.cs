using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using MoneyManager.API.IntegrationTests.ControllerTestUtils;
using MoneyManager.Application.Functions.Users.Commands.LoginUser;
using MoneyManager.Application.Functions.Users.Commands.RegisterUser;
using MoneyManager.Domain.Authentication;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MoneyManager.API.IntegrationTests.ControllerTests.AccountControllerTests
{
    public class LoginUserTests : ControllerTestBase
    {
        public LoginUserTests(WebApplicationFactory<Startup> factory) : base(factory)
        { }

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
