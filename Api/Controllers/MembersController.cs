using Application.Constants;
using Application.Features.TenantMembers.Commands.InviteTenantMember;
using Application.Features.TenantMembers.Queries.GetCurrentTenantMember;
using Application.Features.TenantMembers.Queries.GetTenantMembers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/tenant/[Controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = AuthConstants.ApiScheme)]
    public class MembersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MembersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetTenantMembers(CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetTenantMembersQuery(), cancellationToken);
            return Ok(result);
        }


        [HttpGet("current")]
        public async Task<IActionResult> GetCurrent(CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetCurrentTenantMemberQuery(), cancellationToken);
            return Ok(result);
        }


        [HttpPost("invite")]
        public async Task<IActionResult> Invite([FromBody] InviteTenantMemberCommand inviteTenantMemberCommand, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(inviteTenantMemberCommand, cancellationToken);
            return result.Match(
                success => Ok(success),
                error => StatusCode((int)error.HttpStatusCode, error.Message)
            );
        }
    }
}
