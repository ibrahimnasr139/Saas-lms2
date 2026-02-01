using Application.Constants;
using Application.Contracts.Files;
using Application.Features.Files.Dtos;
using Domain.Enums;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Web;
using IOFile = System.IO.File;

namespace Infrastructure.Services
{
    internal class FileService : IFileService
    {
        private readonly IOptions<Common.Options.FileOptions> _fileOptions;
        private readonly IOptions<BunnyOptions> _bunnyOptions;
        private readonly IOptions<AiTranscriptionOptions> _aiOptions;
        private readonly HttpClient _httpClient;

        public FileService(
            IOptions<Common.Options.FileOptions> fileOptions,
            IOptions<BunnyOptions> bunnyOptions,
            IOptions<AiTranscriptionOptions> aiOptions,
            HttpClient httpClient)
        {
            _fileOptions = fileOptions;
            _bunnyOptions = bunnyOptions;
            _httpClient = httpClient;
            _aiOptions = aiOptions;
        }

        public long GetMaxSize(FileType fileType) => fileType switch
        {
            FileType.Video => _fileOptions.Value.VideoMaxSize,
            FileType.Image => _fileOptions.Value.ImageMaxSize,
            _ => _fileOptions.Value.DocumentMaxSize
        };
        public string GetPath(string fileId, string name, string? folder, string originalFileName)
        {
            var sanitizedFolder = string.IsNullOrWhiteSpace(folder) ? string.Empty : folder.Trim('/');
            var sanitizedFileName = name.Replace(" ", "_");
            var extension = Path.GetExtension(originalFileName);

            return string.IsNullOrEmpty(sanitizedFolder)
                ? $"{fileId}_{sanitizedFileName}{extension}"
                : $"{sanitizedFolder}/{fileId}_{sanitizedFileName}{extension}";
        }
        public FileType GetFileType(string contentType) => contentType switch
        {
            var ct when ct.StartsWith(FileConstants.Image) => FileType.Image,
            var ct when ct.StartsWith(FileConstants.Video) => FileType.Video,
            _ => FileType.Document
        };
        public Task<string?> UploadAsync(IFormFile file, string? folder, string fileId, string path,
            bool embeddingEnabled, CancellationToken cancellationToken)
        {
            var fileType = GetFileType(file.ContentType);

            if (fileType != FileType.Video)
                return UploadFileAsync(file, path);

            return embeddingEnabled
                ? UploadVideoWithAiTranscriptAsync(file, folder, fileId, cancellationToken)
                : UploadVideoAsync(file, folder, cancellationToken);
        }


        #region Private Methods -> File Upload
        private async Task<string?> UploadFileAsync(IFormFile file, string path)
        {
            var uploadUrl = CreateUploadUrl(path);

            using var stream = file.OpenReadStream();
            using var content = new StreamContent(stream);
            content.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);

            using var request = new HttpRequestMessage(HttpMethod.Put, uploadUrl) { Content = content };
            request.Headers.Add(FileConstants.AccessKey, _bunnyOptions.Value.AccessKey);

            var response = await _httpClient.SendAsync(request);

            return response.IsSuccessStatusCode ? CreateCdnUrl(path) : null;
        }
        private async Task<string?> UploadVideoAsync(IFormFile video, string? folder, CancellationToken cancellationToken)
        {
            var tempVideoPath = await SaveTempFileAsync(video, cancellationToken);
            try
            {
                return await UploadVideoToBunnyStreamAsync(tempVideoPath, folder, cancellationToken);
            }
            finally
            {
                DeleteTempFile(tempVideoPath);
            }
        }
        private async Task<string?> UploadVideoWithAiTranscriptAsync(IFormFile video, string? folder, string fileId,
            CancellationToken cancellationToken)
        {
            var tempVideoPath = await SaveTempFileAsync(video, cancellationToken);
            string? audioPath = null;

            try
            {
                var cdnUrl = await UploadVideoToBunnyStreamAsync(tempVideoPath, folder, cancellationToken);
                if (cdnUrl == null)
                    return null;

                audioPath = await ExtractAudioAsync(tempVideoPath, cancellationToken);

                var jobId = Guid.NewGuid().ToString();
                await SendAudioToAiAsync(audioPath, fileId, jobId, cancellationToken);

                return cdnUrl;
            }
            finally
            {
                DeleteTempFile(tempVideoPath);
                DeleteTempFile(audioPath);
            }
        }
        #endregion


        #region Private Methods -> Bunny Stream
        private async Task<string?> UploadVideoToBunnyStreamAsync(string videoPath, string? folder, CancellationToken cancellationToken)
        {
            var videoGuid = await CreateBunnyVideoAsync(videoPath, folder, cancellationToken);
            if (videoGuid == null)
                return null;

            var uploadSuccess = await UploadVideoContentAsync(videoPath, videoGuid, cancellationToken);
            if (!uploadSuccess)
                return null;

            var extension = Path.GetExtension(videoPath).TrimStart('.');
            return $"{FileConstants.BunnyEmbedBaseUrl}/{_bunnyOptions.Value.VideoLibraryId}/{videoGuid}.{extension}";
        }
        private async Task<string?> CreateBunnyVideoAsync(string videoPath, string? folder, CancellationToken cancellationToken)
        {
            var fileName = Path.GetFileName(videoPath);
            var title = string.IsNullOrWhiteSpace(folder)
                ? Path.GetFileNameWithoutExtension(fileName)
                : $"{folder}/{Path.GetFileNameWithoutExtension(fileName)}";

            using var request = new HttpRequestMessage(HttpMethod.Post,
                $"{FileConstants.BunnyStreamBaseUrl}/{_bunnyOptions.Value.VideoLibraryId}/{FileConstants.Videos}")
            {
                Content = JsonContent.Create(new { title })
            };
            request.Headers.Add(FileConstants.AccessKey, _bunnyOptions.Value.StreamKey);

            var response = await _httpClient.SendAsync(request, cancellationToken);
            if (!response.IsSuccessStatusCode)
                return null;

            var json = await response.Content.ReadAsStringAsync(cancellationToken);
            var videoInfo = JsonSerializer.Deserialize<StreamUploadResponse>(json);

            return string.IsNullOrWhiteSpace(videoInfo?.guid) ? null : videoInfo.guid;
        }
        private async Task<bool> UploadVideoContentAsync(string videoPath, string videoGuid, CancellationToken cancellationToken)
        {
            await using var stream = IOFile.OpenRead(videoPath);
            using var content = new StreamContent(stream);
            content.Headers.ContentType = new MediaTypeHeaderValue(FileConstants.OctetStream);
            content.Headers.ContentLength = stream.Length;

            using var request = new HttpRequestMessage(HttpMethod.Put,
                $"{FileConstants.BunnyStreamBaseUrl}/{_bunnyOptions.Value.VideoLibraryId}/{FileConstants.Videos}/{videoGuid}")
            {
                Content = content
            };
            request.Headers.Add(FileConstants.AccessKey, _bunnyOptions.Value.StreamKey);

            var response = await _httpClient.SendAsync(request, cancellationToken);
            return response.IsSuccessStatusCode;
        }
        #endregion


        #region Private Methods -> Audio & AI
        private async Task<string> ExtractAudioAsync(string videoPath, CancellationToken cancellationToken)
        {
            var audioPath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}.mp3");

            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "ffmpeg",
                    Arguments = $"-i \"{videoPath}\" -vn -q:a 5 -map a \"{audioPath}\" -y",
                    RedirectStandardError = true,
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };

            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            process.Start();

            var stderrTask = process.StandardError.ReadToEndAsync(cancellationToken);

            await process.WaitForExitAsync(cancellationToken);
            stopwatch.Stop();

            if (process.ExitCode != 0)
            {
                var error = await stderrTask;
                throw new Exception($"FFmpeg error: {error}");
            }

            return audioPath;
        }
        private async Task SendAudioToAiAsync(string audioPath, string fileId, string jobId, CancellationToken cancellationToken)
        {
            using var content = new MultipartFormDataContent();
            await using var audioStream = IOFile.OpenRead(audioPath);

            content.Add(new StreamContent(audioStream), FileConstants.AiRequestAudio, Path.GetFileName(audioPath));
            content.Add(new StringContent(fileId), FileConstants.AiRequestFileId);
            content.Add(new StringContent(jobId), FileConstants.AiRequestJobId);
            content.Add(new StringContent(_aiOptions.Value.CallbackUrl), FileConstants.AiRequestCallBackUrl);

            var response = await _httpClient.PostAsync(_aiOptions.Value.Url, content, cancellationToken);
            response.EnsureSuccessStatusCode();
        }
        #endregion


        #region Private Methods -> Helpers
        private string CreateCdnUrl(string path)
        {
            var encodedPath = HttpUtility.UrlPathEncode(path);
            return $"{_bunnyOptions.Value.CdnUrl.TrimEnd('/')}/{encodedPath.TrimStart('/')}";
        }
        private string CreateUploadUrl(string path)
        {
            var encodedPath = HttpUtility.UrlPathEncode(path);
            return $"https://{_bunnyOptions.Value.HostName}/{_bunnyOptions.Value.StorageZoneName}/{encodedPath.TrimStart('/')}";
        }
        private async Task<string> SaveTempFileAsync(IFormFile file, CancellationToken cancellationToken)
        {
            var safeFileName = Path.GetFileName(file.FileName);
            var tempPath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}_{safeFileName}");

            await using var fs = new FileStream(tempPath, FileMode.Create, FileAccess.Write, FileShare.None,
                bufferSize: 1024 * 1024, useAsync: true);

            await file.CopyToAsync(fs, cancellationToken);
            return tempPath;
        }
        private static void DeleteTempFile(string? path)
        {
            if (path != null && IOFile.Exists(path))
                IOFile.Delete(path);
        }
        #endregion
    }
}
