namespace Application.Helpers
{
    public static class EmailConfirmationHelper
    {
        public static string GenerateEmailBodyHelper(string template, Dictionary<string, string> placeholders)
        {
            var basePath = AppContext.BaseDirectory;
            var path = Path.Combine(basePath, "Templates", $"{template}.html");
            var body = System.IO.File.ReadAllText(path);
            foreach (var placeholder in placeholders)
            {
                body = body.Replace(placeholder.Key, placeholder.Value);
            }
            return body;
        }
    }
}
