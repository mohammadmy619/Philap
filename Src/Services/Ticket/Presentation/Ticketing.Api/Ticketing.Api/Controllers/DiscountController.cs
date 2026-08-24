using Application.Discounts;
using Application.Discounts.GetAllDiscounts;
using Application.Discounts.GetDiscount;
using Application.Discounts.GetDiscountUsagesPaged;
using Application.Ticketing;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ticketing.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiscountController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DiscountController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// ایجاد تخفیف جدید
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateDiscount([FromBody] CreateDiscountCommand command)
        {
            var discountId = await _mediator.Send(command);

            return CreatedAtAction(nameof(GetDiscountById), new { discountId }, discountId);
        }

        /// <summary>
        /// دریافت اطلاعات تخفیف بر اساس شناسه
        /// </summary>
        [HttpGet("{discountId}")]
        [ProducesResponseType(typeof(GetDiscountResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetDiscountById(Guid discountId)
        {
            var query = new GetDiscountByIdQuery(discountId);
            var result = await _mediator.Send(query);

            return Ok(result);
        }

        /// <summary>
        /// دریافت لیست تخفیف‌ها با صفحه‌بندی
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(GetDiscountsResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDiscounts([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var query = new GetDiscountsQuery(page, pageSize);
            var result = await _mediator.Send(query);

            return Ok(result);
        }

        /// <summary>
        /// بروزرسانی اطلاعات تخفیف
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(DiscountUpdatedResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateDiscount(Guid id, [FromBody] UpdateDiscountCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest("Discount ID mismatch");
            }

            var result = await _mediator.Send(command);

            return Ok(result);
        }

        /// <summary>
        /// دریافت لیست استفاده‌های تخفیف بر اساس شناسه تخفیف
        /// </summary>
        [HttpGet("{discountId}/usages")]
        [ProducesResponseType(typeof(IEnumerable<GetDiscountUsageResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDiscountUsages(Guid discountId)
        {
            var query = new GetDiscountUsagesByDiscountIdQuery(discountId);
            var result = await _mediator.Send(query);

            return Ok(result);
        }

        /// <summary>
        /// دریافت لیست استفاده‌های تخفیف با صفحه‌بندی
        /// </summary>
        [HttpGet("{discountId}/usages/paged")]
        [ProducesResponseType(typeof(GetDiscountUsagesPagedResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDiscountUsagesPaged(
            Guid discountId,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20)
        {
            var query = new GetDiscountUsagesPagedQuery(discountId, page, pageSize);
            var result = await _mediator.Send(query);

            return Ok(result);
        }
    }
}