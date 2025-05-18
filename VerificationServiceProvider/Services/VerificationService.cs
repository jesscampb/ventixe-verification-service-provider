using VerificationServiceProvider.Data.Contexts;
using VerificationServiceProvider.Models;

namespace VerificationServiceProvider.Services;

public class VerificationService(VerificationDbContext context)
{
    private static readonly Random _random = new();
    private readonly VerificationDbContext _context = context;

    public static VerificationCodeModel GenerateVerificationCode(string email, int expiresInMinutes = 5)
    {
        var verificationCode = new VerificationCodeModel
        {
            Email = email,
            Code = _random.Next(100000, 999999).ToString(),
            ExpiresAt = DateTime.UtcNow.AddMinutes(expiresInMinutes)
        };

        return verificationCode;
    }

    public async Task<bool> SaveVerificationCodeAsync(VerificationCodeModel verificationCode)
    {
        try
        {
            _context.Add(verificationCode);
            await _context.SaveChangesAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }
}
