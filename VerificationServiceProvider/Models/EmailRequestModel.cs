namespace VerificationServiceProvider.Models;

public class EmailRequestModel
{
    public IEnumerable<string> Recipients { get; set; } = Enumerable.Empty<string>();
    public string Subject { get; set; } = "No Subject";
    public string PlainText { get; set; } = string.Empty;
    public string Html { get; set; } = string.Empty;
}
