using Application.Ticketing;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ticketing.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController(IMediator _mediator) : ControllerBase
    {
        [HttpGet("{bookingId}")]
        [ProducesResponseType(typeof(GetBookingResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetBookingById(Guid bookingId)
        {
            var query = new GetBookingByIdQuery(bookingId);
            var result = await _mediator.Send(query);

            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(CreateBookingResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateBooking([FromBody] CreateBookingCommand command)
        {
            var result = await _mediator.Send(command);

            return CreatedAtAction(nameof(GetBookingById), new { bookingId = result.BookingId }, result);
        }

        [HttpPut("{bookingId}")]
        [ProducesResponseType(typeof(UpdateBookingResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateBooking(Guid bookingId, [FromBody] UpdateBookingCommand command)
        {
            // اطمینان از تطابق BookingId در URL و Body
            if (bookingId != command.BookingId)
            {
                return BadRequest("BookingId mismatch");
            }

            var result = await _mediator.Send(command);

            return Ok(result);
        }

        [HttpPatch("{bookingId}/cancel")]
        [ProducesResponseType(typeof(CancelBookingResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CancelBooking(Guid bookingId)
        {
            var command = new CancelBookingCommand(bookingId);
            var result = await _mediator.Send(command);

            return Ok(result);
        }
    }
}
