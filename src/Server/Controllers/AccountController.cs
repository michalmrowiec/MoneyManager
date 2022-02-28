using MoneyManager.Server.Services;
using MoneyManager.Shared;
using Microsoft.AspNetCore.Mvc;

namespace MoneyManager.Server.Controllers
{
    [ApiController]
    [Route("tapi/account")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost]
        [Route("register")]
        public ActionResult RegisterUser([FromBody] RegisterUserDto registerUserDto)
        {
            _accountService.RegisterUser(registerUserDto);
            return Ok();
        }

        [HttpPost]
        [Route("login")]
        public ActionResult LoginUser([FromBody] LoginUserDto loginUserDto)
        {
            var token = _accountService.GenerateJwtToken(loginUserDto);
            return Ok(token);
        }
    }
}
