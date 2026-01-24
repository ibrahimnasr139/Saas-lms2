using Application.Constants;
using Application.Features.Files.Commands.UploadFile;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = AuthConstants.ApiScheme)]
    public class FilesController : ControllerBase
    {
        private readonly IMediator _mediator;
        public FilesController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile([FromBody] UploadFileCommand uploadFileCommand)
        {
            var result = await _mediator.Send(uploadFileCommand);
            return Ok(result);
        }

    }
}
