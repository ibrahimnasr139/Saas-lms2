using Application.Features.Plan.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlansController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PlansController(IMediator mediator)
        {
            this._mediator = mediator;
        }

        [HttpGet("")]
        public async Task<IActionResult> Get()
        {
            var result = await _mediator.Send(new GetAllPlansQuery());
            return Ok(result);
        }
    }
}
