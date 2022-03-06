using MediatR;
using Microsoft.AspNetCore.Mvc;
using MoneyManager.Application.Functions.Users.Commands.LoginUser;
using MoneyManager.Application.Functions.Users.Commands.RegisterUser;
using MoneyManager.Domain.Authentication;

namespace MoneyManager.API.Controllers
{
    [ApiController]
    [Route("api/account")]
    public class AccountController : ControllerBase
    {
        private readonly IMediator _mediator;
        public AccountController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Route("register")]
        public async Task<ActionResult<UserToken>> RegisterUser([FromBody] RegisterUserCommand registerUserDto)
        {
            return Ok(await _mediator.Send(registerUserDto));
        }

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<UserToken>> LoginUser([FromBody] LoginUserCommand loginUserDto)
        {
            return Ok(await _mediator.Send(loginUserDto));
        }
    }
}
