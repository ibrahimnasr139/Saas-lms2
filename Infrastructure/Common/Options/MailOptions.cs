namespace Infrastructure.Common.Options
{
    public sealed class MailOptions
    {
        public string Email { get; set; } = string.Empty;
        public string FromName { get; set; } = string.Empty;
        public string BrevoApiKey { get; set; } = string.Empty;
        public string BaseUrl { get; set; } = string.Empty;
        public string Endpoint { get; set; } = string.Empty;
    }
}
