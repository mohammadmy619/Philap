using Application.User.CreateUser;
using Application.User.GetUser;
using Application.User.UpdateUser;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(IMediator _mediator) : ControllerBase
    {


        [HttpGet("{userId}")]
        [Authorize]
        [ProducesResponseType(typeof(GetUserResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUserById()
        {
            var query = new GetUserQuery();
            var result = await _mediator.Send(query);

            return Ok(result);

        }

      

        [HttpPut]
        [Authorize]
        [ProducesResponseType(typeof(UpdateUserResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserCommand command)
        {
         
            var result = await _mediator.Send(command);

            return Ok(result);
        }

    }
}
