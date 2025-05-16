using VerificationServiceProvider.Models;

namespace VerificationServiceProvider.Services;

public class VerificationService
{
    private static readonly Random _random = new();

    public static VerificationCode GenerateVerificationCode(string email, int expiresInMinutes = 5)
    {
        var verificationCode = new VerificationCode
        {
            Email = email,
            Code = _random.Next(100000, 999999).ToString(),
            ExpiresAt = DateTime.UtcNow.AddMinutes(expiresInMinutes)
        };

        return verificationCode;
    }
}
