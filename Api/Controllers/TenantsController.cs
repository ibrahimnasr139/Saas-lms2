using Application.Constants;
using Application.Features.Tenants.Queries.GetLastTenant;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = AuthConstants.ApiScheme)]

    public class TenantsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TenantsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetLastTenant(CancellationToken cancellationToken)
        {
            var tenant = await _mediator.Send(new GetLastTenantQueriy(), cancellationToken);
            return Ok(tenant);
        }
    }
}
