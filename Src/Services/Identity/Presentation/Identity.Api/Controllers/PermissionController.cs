using Application.Permission.CreatePermission;
using Application.Permission.GetAccess;
using Application.Permission.GetPermission;
using Application.Permission.UpdatePermission;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionController(IMediator _mediator) : ControllerBase
    {

        /// <summary>
        /// دریافت جزئیات یک Permission بر اساس ID
        /// </summary>
        [HttpGet("{permissionId:guid}")]
        [ProducesResponseType(typeof(GetPermissionResponse), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetPermission(
            [FromRoute] Guid permissionId,
            CancellationToken cancellationToken)
        {
         
                var query = new GetPermissionQuery(permissionId);
                var response = await _mediator.Send(query, cancellationToken);

                return Ok(response);
       
        }

        /// <summary>
        /// ایجاد یک Permission جدید
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(CreatePermissionResponse), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreatePermission(
            [FromBody] CreatePermissionCommand command,
            CancellationToken cancellationToken)
        {
       
             var response = await _mediator.Send(command, cancellationToken);

                return Ok(response);
     
        }
        /// <summary>
        /// به‌روزرسانی یک permission موجود
        /// </summary>
        [HttpPut("{id:guid}")]
        [ProducesResponseType(typeof(UpdatePermissionResponse), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdatePermission(
            [FromBody] UpdatePermissionCommand command,
            CancellationToken cancellationToken)
        {
            
                var response = await _mediator.Send(command, cancellationToken);
                return Ok(response);
  
        }

    }
}
