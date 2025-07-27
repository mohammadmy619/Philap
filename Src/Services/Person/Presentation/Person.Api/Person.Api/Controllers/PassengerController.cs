using Application.Passenger.GetPassengers;
using Application.Passenger.UpdatePassenger;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Person.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PassengerController(IMediator _mediator) : ControllerBase
    {
        [HttpPost]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreatePassenger([FromBody] CreatePassengerCommand command)
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetPassengerById), new { id = result }, result);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(GetPassengerByIdResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPassengerById(Guid id)
        {
            var query = new GetPassengerByIdQuery(id);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdatePassenger([FromBody] UpdatePassengerCommand command)
        {
            await _mediator.Send(command);
            return NoContent();
        }

  

        [HttpGet]
        [ProducesResponseType(typeof(List<GetAllPassengersQuery>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllPassengers([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var query = new GetAllPassengersQuery(pageNumber, pageSize);
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}
