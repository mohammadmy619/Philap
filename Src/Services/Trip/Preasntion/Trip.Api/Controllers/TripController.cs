using Application.Trips.CreateTrip;
using Application.Trips.GetTrip;
using Application.Trips.GetTripsWithPagination;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Trip.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TripController(IMediator _mediator) : ControllerBase
    {

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var query = new GetTripQuery(id);
            var response = await _mediator.Send(query);
            return Ok(response);
        }
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] GetTripsWithPaginationQuery query)
        {
            var response = await _mediator.Send(query);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTripCommand command)
        {

            var trip = await _mediator.Send(command);
            return Ok(trip);

        }
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update([FromBody] UpdateTripCommand command)
        {
            await _mediator.Send(command);
            return NoContent();

        }
    }
}
