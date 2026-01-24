using Application.Constants;
using Application.Features.Users.Queries.GetProfile;
using Application.Features.Users.Queries.GetTenants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = AuthConstants.ApiScheme)]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;
        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet("me")]
        public async Task<IActionResult> GetProfile(CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetProfileQuery(), cancellationToken);
            return Ok(result);
        }


        [HttpGet("me/tenants")]
        public async Task<IActionResult> GetTenants(CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetTenantsQuery(), cancellationToken);
            return Ok(result);
        }
    }
}
