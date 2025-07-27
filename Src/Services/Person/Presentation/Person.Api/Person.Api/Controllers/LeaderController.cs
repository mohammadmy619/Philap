using Application.Leader.GetLeaderById;
using Application.Leader.GetLeaders;
using Application.Leader.UpdateLeader;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Person.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeaderController(IMediator _mediator) : ControllerBase
    {
        [HttpPost]
        [ProducesResponseType(typeof(CreateLeaderResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateLeader([FromBody] CreateLeaderCommand command)
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetLeaderById), new { id = result.LeaderId }, result);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(GetLeaderByIdResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetLeaderById(Guid id)
        {
            var query = new GetLeaderByIdQuery(id);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateLeader([FromBody] UpdateLeaderCommand command)
        {
            await _mediator.Send(command);
            return NoContent();
        }

 

        [HttpGet]
        [ProducesResponseType(typeof(GetLeadersResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllLeaders([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var query = new GetLeadersQuery(pageNumber, pageSize);
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}
