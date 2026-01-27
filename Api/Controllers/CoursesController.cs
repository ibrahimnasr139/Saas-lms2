using Application.Constants;
using Application.Features.Courses.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/tenant/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = AuthConstants.ApiScheme)]
    public class CoursesController : ControllerBase
    {
        private readonly IMediator _mediator;
        public CoursesController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet("statistics")]
        public async Task<IActionResult> GetStatistics()
        {
            var result = await _mediator.Send(new GetStatisticsQuery());
            return Ok(result);
        }
    }
}
