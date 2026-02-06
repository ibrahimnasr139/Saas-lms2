using Application.Constants;
using Application.Features.Tenants.Commands.DeleteContentLibraryResource;
using Application.Features.Tenants.Queries.GetContentLibraryResources;
using Application.Features.Tenants.Queries.GetContentLibraryStatistics;
using Application.Features.Tenants.Queries.GetLastTenant;
using Application.Features.Tenants.Queries.GetTenantUsage;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/tenant")]
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
            var tenant = await _mediator.Send(new GetLastTenantQuery(), cancellationToken);
            return Ok(tenant);
        }


        [HttpGet("usage")]
        public async Task<IActionResult> GetTenantUsage(CancellationToken cancellationToken)
        {
            var usage = await _mediator.Send(new GetTenantUsageQuery(), cancellationToken);
            return Ok(usage);
        }


        [HttpGet("content-library/resources")]
        public async Task<IActionResult> GetContentLibraryResources([FromQuery] string? q, [FromQuery] string type, CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(new GetContentLibraryResourcesQuery(q, type), cancellationToken);
            return Ok(response);
        }


        [HttpGet("content-library/statistics")]
        public async Task<IActionResult> GetContentLibraryStatistics(CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(new GetContentLibraryStatisticsQuery(), cancellationToken);
            return Ok(response);
        }


        [HttpDelete("content-library/resources/{id}")]
        public async Task<IActionResult> DeleteContentLibraryResource([FromRoute] string id, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new DeleteContentLibraryResourceCommand(id), cancellationToken);

            return result.Match<IActionResult>(
                success => Ok(success),
                error => StatusCode((int)error.HttpStatusCode, error.Message)
            );
        }
    }
}
