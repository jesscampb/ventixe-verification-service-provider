namespace VerificationServiceProvider.Models;

public class EmailRequestModel
{
    public List<string> Recipients { get; set; } = new();
    public string Subject { get; set; } = "No Subject";
    public string PlainText { get; set; } = string.Empty;
    public string Html { get; set; } = string.Empty;
}
