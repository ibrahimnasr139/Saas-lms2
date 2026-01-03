using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.Files.Dtos
{
    public sealed class UploadFileDto
    {
        public string UploadUrl { get; init; } = string.Empty;
        public string CdnUrl { get; init; } = string.Empty;
        public string Path { get; init; } = string.Empty;
        public Dictionary<string, string>? Metadata { get; init; }
    }
}
