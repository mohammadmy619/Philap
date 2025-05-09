using Application.Permission.CreateAccessControl;
using Application.Permission.CreatePermission;
using Application.Permission.GetAccess;
using Application.Permission.UpdateAccessControl;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccessControlController(IMediator _mediator) : ControllerBase
    {
        /// دریافت لیست دسترسی‌ها برای یک permissionId
        /// </summary>
        [HttpGet("access-control/{permissionId:guid}")]
        [ProducesResponseType(typeof(List<GetAccessControlResponse>), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAccessControl(
            [FromRoute] Guid permissionId,
            CancellationToken cancellationToken)
        {

            var query = new GetAccessControlQuery(permissionId);
            var response = await _mediator.Send(query, cancellationToken);

            return Ok(response);

        }


        /// <summary>
        /// ایجاد یک دسترسی جدید برای یک permission مشخص
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(CreateAccessControlResponse), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateAccessControl(
            [FromBody] CreateAccessControlCommand command,
            CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(command, cancellationToken);
            return Ok(response);
        }


        /// <summary>
        /// به‌روزرسانی یک دسترسی موجود برای یک permission
        /// </summary>
        [HttpPut]
        [ProducesResponseType(typeof(UpdateAccessControlResponse), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateAccessControl(
            [FromBody] UpdateAccessControlCommand command,
            CancellationToken cancellationToken)
        {

            var response = await _mediator.Send(command, cancellationToken);

            return Ok(response);

        }

    
    }

}