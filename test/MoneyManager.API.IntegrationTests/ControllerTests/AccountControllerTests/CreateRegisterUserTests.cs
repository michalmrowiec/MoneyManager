using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using MoneyManager.API.IntegrationTests.ControllerTestUtils;
using MoneyManager.Application.Functions.Users.Commands.RegisterUser;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MoneyManager.API.IntegrationTests.ControllerTests.AccountControllerTests
{
    public class CreateRegisterUserTests : ControllerTestBase
    {
        public CreateRegisterUserTests(WebApplicationFactory<Startup> factory) : base(factory)
        { }

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
        public static IEnumerable<object[]> TestRegisterUsers_InvalidModel_SameEmailAddresses => new List<object[]>
        {
            new object[]
            {
                new RegisterUserCommand { Name = "Test123", Email = "kjsdnfgjndfg@test234.com", Password = "!@#$FDHfdgj45", RepeatPassword = "!@#$FDHfdgj45" },
                new RegisterUserCommand { Name = "Test123", Email = "kjsdnfgjndfg@test234.com", Password = "!@#$FDHfdgj45", RepeatPassword = "!@#$FDHfdgj45" }
            },
            new object[]
            {
                new RegisterUserCommand { Name = "123test", Email = "test1op@test.com", Password = "dshdsegdfjn", RepeatPassword = "dshdsegdfjn" },
                new RegisterUserCommand { Name = "Thomas 12", Email = "test1op@test.com", Password = "47546dfjn", RepeatPassword = "47546dfjn" }
            },
            new object[]
            {
                new RegisterUserCommand { Name = "Json Carl", Email = "address123a@iop.pl", Password = "da3%2Zg?52", RepeatPassword = "da3%2Zg?52" },
                new RegisterUserCommand { Name = "Adam von Gen", Email = "address123a@iop.pl", Password = "5hdsHG@$", RepeatPassword = "5hdsHG@$" }
            },
            new object[]
            {
                new RegisterUserCommand { Name = "@#$TesT", Email = "test4@gmail.com", Password = "34$fsdh#$G", RepeatPassword = "34$fsdh#$G" },
                new RegisterUserCommand { Name = "@#$TesT", Email = "test4@gmail.com", Password = "34$fsdh#$G", RepeatPassword = "34$fsdh#$G" }
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
        [MemberData(nameof(TestRegisterUsers_InvalidModel_SameEmailAddresses))]
        public async Task CreateUser_WithInvalidModelSameEmailAddresses_ReturnsBadRequest(RegisterUserCommand firstUser, RegisterUserCommand user)
        {
            var jsonFU = JsonConvert.SerializeObject(firstUser);
            var httpContentFU = new StringContent(jsonFU, UnicodeEncoding.UTF8, "application/json");
            await _httpClient.PostAsync("/api/account/register/", httpContentFU);


            var json = JsonConvert.SerializeObject(user);
            var httpContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

            var respond = await _httpClient.PostAsync("/api/account/register/", httpContent);

            respond.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}
