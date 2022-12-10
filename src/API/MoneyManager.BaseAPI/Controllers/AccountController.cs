using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoneyManager.Application.Functions.Users.Commands.ChangePasswordUser;
using MoneyManager.Application.Functions.Users.Commands.LoginUser;
using MoneyManager.Application.Functions.Users.Commands.RegisterUser;
using MoneyManager.Application.Functions.Users.Commands.SendForgotPasswordEmail;
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
            var response = await _mediator.Send(registerUserDto);
            if(!response.Success)
                return BadRequest(response);
            return Ok(response.UserToken);
        }

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<UserToken>> LoginUser([FromBody] LoginUserCommand loginUserDto)
        {
            var response = await _mediator.Send(loginUserDto);
            if(!response.Success)
                return BadRequest(response);
            return Ok(response.UserToken);
        }

        [HttpPost]
        [Route("forgotpassword")]
        public async Task<ActionResult> SendForgotPasswordEmail([FromBody] SendForgotPasswordEmailCommand sendForgotPasswordEmail)
        {
            var emailSend = await _mediator.Send(sendForgotPasswordEmail);
            return emailSend ? Ok(emailSend) : BadRequest();
        }

        [Authorize]
        [HttpPut]
        public async Task<ActionResult> ChangePasswordUser([FromBody] ChangePasswordUserCommand changePasswordUser)
        {
            var result = await _mediator.Send(changePasswordUser);
            return result ? Ok() : BadRequest();
        }
    }
}
