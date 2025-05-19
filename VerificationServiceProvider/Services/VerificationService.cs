using Microsoft.EntityFrameworkCore;
using VerificationServiceProvider.Data.Contexts;
using VerificationServiceProvider.Mappers;
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
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.AddMinutes(expiresInMinutes)
        };

        return verificationCode;
    }

    public async Task<bool> SaveVerificationCodeAsync(VerificationCodeModel verificationCode)
    {
        try
        {
            var entity = verificationCode.ToEntity();
            _context.Add(entity);
            await _context.SaveChangesAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<EmailRequestModel> GenerateVerificationCodeEmailAsync(string email)
    {
        var verificationCode = GenerateVerificationCode(email);
        var result = await SaveVerificationCodeAsync(verificationCode);

        if (result)
        {
            var emailRequest = new EmailRequestModel
            {
                Recipients = [email],
                Subject = $"Verification Code: {verificationCode}",
                PlainText = $"Your verification code is: {verificationCode}",
                Html = $"<html><body><h1>Verification Code</h1><p>Here is your verification code:</p><p>{verificationCode}</p></body><html>"
            };

            return emailRequest;
        }

        return null!;
    }

    public async Task<bool> ValidateVerificationCodeAsync(ValidateRequestModel? validateRequest)
    {
        var entity = await _context.VerificationCodes.FirstOrDefaultAsync(x =>
            x.Email == validateRequest!.Email &&
            x.Code == validateRequest!.Code &&
            x.ExpiresAt > DateTime.UtcNow
        );

        if (entity != null)
        {
            _context.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        return false;
    }
}
