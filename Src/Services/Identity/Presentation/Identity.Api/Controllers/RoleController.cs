using Application.Role.CreateRole;
using Application.Role.GetRole;
using Application.Role.UpdateRole;
using Domain.RoleAgregate.Exception;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController(IMediator _mediator) : ControllerBase
    {
        /// <summary>
        /// دریافت نقش ها
        /// </summary>
        [HttpGet]
        [Authorize]
        [ProducesResponseType(typeof(GetRoleResponse), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetUserRoles(CancellationToken cancellationToken)
        {

            var query = new GetRoleQuery();
            var response = await _mediator.Send(query, cancellationToken);
            return Ok(response);
        }

        /// <summary>
        /// ایجاد یک نقش جدید با مجوزهای مشخص شده
        /// </summary>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(typeof(CreateRoleResponse), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateRole(
            [FromBody] CreateRoleCommand command,
            CancellationToken cancellationToken)
        {

            var response = await _mediator.Send(command, cancellationToken);

            return Ok(response);

        }

        /// <summary>
        /// به‌روزرسانی یک نقش موجود
        /// </summary>
        [HttpPut("{roleId:guid}")]
        [Authorize]
        [ProducesResponseType(typeof(UpdateRoleResponse), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateRole(
            [FromBody] UpdateRoleCommand command,
            CancellationToken cancellationToken)
        {

            var response = await _mediator.Send(command, cancellationToken);

            return Ok(response);

        }
    }
}
