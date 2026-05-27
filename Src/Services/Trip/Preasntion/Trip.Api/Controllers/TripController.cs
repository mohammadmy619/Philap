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
    public class TripController(IMediator mediator) : ControllerBase
    {
        /// <summary>
        /// دریافت اطلاعات یک سفر بر اساس شناسه
        /// </summary>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(GetTripResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get(
            [FromRoute] Guid id,
            CancellationToken cancellationToken)
        {
            var query = new GetTripQuery(id);
            var response = await mediator.Send(query, cancellationToken);
            return Ok(response);
        }

        /// <summary>
        /// دریافت لیست سفرها با قابلیت صفحه‌بندی
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IReadOnlyCollection<GetTripsResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAll(
            [FromQuery] GetTripsWithPaginationQuery query,
            CancellationToken cancellationToken)
        {
            var response = await mediator.Send(query, cancellationToken);
            return Ok(response);
        }

        /// <summary>
        /// ایجاد یک سفر جدید
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(CreateTripResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create(
            [FromBody] CreateTripCommand command,
            CancellationToken cancellationToken)
        {
            var response = await mediator.Send(command, cancellationToken);
            return Ok(response);
        }

        /// <summary>
        /// به‌روزرسانی اطلاعات یک سفر موجود
        /// </summary>
        [HttpPut("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update(
            [FromRoute] Guid id,
            [FromBody] UpdateTripCommand command,
            CancellationToken cancellationToken)
        {
            // اطمینان از هماهنگی ID در روت و بدنه درخواست
            if (command.TripId != id)
                return BadRequest("TripId in route and body must match.");

            await mediator.Send(command, cancellationToken);
            return NoContent();
        }
    }
}