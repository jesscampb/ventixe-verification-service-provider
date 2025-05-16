namespace VerificationServiceProvider.Models;

public class VerificationCodeModel
{
    public string Email { get; set; } = null!;
    public string Code { get; set; } = null!;
    public DateTime ExpiresAt { get; set; }
}
