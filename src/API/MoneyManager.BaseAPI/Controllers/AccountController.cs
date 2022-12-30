using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoneyManager.API.Services;
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
        private readonly IUserContextService _userContextService;

        public AccountController(IMediator mediator, IUserContextService userContextService)
        {
            _mediator = mediator;
            _userContextService = userContextService;
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
            changePasswordUser.UserId = _userContextService.GetUserId;
            var response = await _mediator.Send(changePasswordUser);

            return response.Success ? Ok(response) : BadRequest(response);
        }
    }
}
