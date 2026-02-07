using Application.Constants;
using Application.Features.Roles.Queries.GetTenantRole;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/tenant/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = AuthConstants.ApiScheme)]
    public class RolesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RolesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetRoles(CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetTenantRoleQuery(), cancellationToken);
            return Ok(result);
        }
    }
}
