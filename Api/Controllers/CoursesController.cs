using Application.Constants;
using Application.Features.Courses.Queries.GetAll;
using Application.Features.Courses.Queries.GetStatistics;
using MediatR;
using Microsoft.AspNetCore.Authorization;
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
        public async Task<IActionResult> GetStatistics(CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetStatisticsQuery(), cancellationToken);
            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] GetAllQuery getAllQuery, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(getAllQuery, cancellationToken);
            return Ok(result);
        }
    }
}
