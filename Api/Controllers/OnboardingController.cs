using Application.Constants;
using Application.Features.Tenants.Commands.CreateOnboarding;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = AuthConstants.ApiScheme)]
    public class OnboardingController : ControllerBase
    {
        private readonly IMediator _mediator;
        public OnboardingController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost]
        public async Task<IActionResult> CreateOnboarding([FromBody] CreateOnboardingCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return result.Match<IActionResult>(
                onboardingDto => Created(string.Empty, onboardingDto),
                error => StatusCode((int)error.HttpStatusCode, error.Message));
        }
    }

}
