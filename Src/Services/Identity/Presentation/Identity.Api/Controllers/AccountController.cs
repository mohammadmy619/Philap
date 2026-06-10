using Application.DTO;
using Application.User.CreateUser;
using Application.User.LoginUser;
using Application.User.UpdateUser;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;



namespace Identity.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController(IMediator _mediator) : ControllerBase
    {
        
   
        [HttpPost]
        [ProducesResponseType(typeof(UpdateUserResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterUserCommand Register)
        {
            var result = await _mediator.Send(Register);
            return Ok(result);
        }

        [HttpPost("login")]
        [ProducesResponseType(typeof(LoginUserResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Login([FromBody] LoginUserQuery login)
        {
            var result = await _mediator.Send(login);
            return Ok(result);
        }



        [Authorize]
        [HttpGet("private")]
        public IActionResult Private()
        {
            Console.WriteLine("=== CLAIMS IN CONTROLLER ===");
            foreach (var claim in User.Claims)
            {
                Console.WriteLine($"{claim.Type}: {claim.Value}");
            }

            return Ok(new
            {
                message = "این اندپوینت نیاز به احراز هویت دارد",
                claims = User.Claims.Select(c => new { c.Type, c.Value }).ToList()
            });
        }


    }
}
