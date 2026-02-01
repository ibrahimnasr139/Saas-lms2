using Application.Constants;
using Application.Features.Files.Commands.CallBack;
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
        public async Task<IActionResult> UploadFile([FromForm] UploadFileCommand uploadFileCommand)
        {
            var result = await _mediator.Send(uploadFileCommand);

            return result.Match<IActionResult>(
                success => Ok(success),
                error => StatusCode((int)error.HttpStatusCode, error)
            );
        }


        [HttpPost("callback")]
        public async Task<IActionResult> CallBack([FromBody] CallBackCommand command)
        {
            await _mediator.Send(command);
            return NoContent();
        }
    }
}
