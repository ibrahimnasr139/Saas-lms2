using Application.Contracts.Files;
using FluentValidation;

namespace Application.Features.Files.Commands.UploadFile
{
    public sealed class UploadFileCommandValidator : AbstractValidator<UploadFileCommand>
    {
        private readonly IFileService _fileService;
        public UploadFileCommandValidator(IFileService fileService)
        {
            _fileService = fileService;

            RuleFor(x => x.Size)
                .GreaterThan(0).WithMessage("File size must be greater than zero.");
            RuleFor(x => x)
            .Custom((request, context) =>
            {
                var maxSize = _fileService.GetMaxSize(request.Type);

                if (request.Size > maxSize)
                {
                    context.AddFailure(
                        nameof(request.Size),
                        $"File size exceeds allowed limit for {request.Type}"
                    );
                }
            });
            RuleFor(x => x.Folder)
                .NotEmpty().WithMessage("Folder cannot be empty.");
        }

    }
}
