namespace Application.Features.Auth.Dtos
{
    public sealed class LoginDto
    {
        public string LastActiveTenant { get; set; } = string.Empty;
        public string Message { get; set; } = "success";
    }
}
