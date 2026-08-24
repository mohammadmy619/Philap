using Application.Accountings.GetAccountingById;
using Application.Accountings.GetAllAccountings;
using MediatR;
using Microsoft.AspNetCore.Mvc;



namespace Ticketing.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountingController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AccountingController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// دریافت لیست تمام حساب‌ها
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<GetAccountingResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllAccountings()
        {
            var query = new GetAllAccountingsQuery();
            var result = await _mediator.Send(query);

            return Ok(result);
        }

        /// <summary>
        /// دریافت اطلاعات حساب بر اساس شناسه
        /// </summary>
        [HttpGet("{accountingId}")]
        [ProducesResponseType(typeof(GetAccountingResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAccountingById(Guid accountingId)
        {
            var query = new GetAccountingByIdQuery(accountingId);
            var result = await _mediator.Send(query);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }
    }
}
