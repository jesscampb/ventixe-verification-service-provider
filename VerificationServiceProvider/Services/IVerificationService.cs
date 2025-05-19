using VerificationServiceProvider.Models;

namespace VerificationServiceProvider.Services
{
    public interface IVerificationService
    {
        Task<EmailRequestModel> GenerateVerificationCodeEmailAsync(string email);
        Task<bool> SaveVerificationCodeAsync(VerificationCodeModel verificationCode);
        Task<bool> ValidateVerificationCodeAsync(ValidateRequestModel? validateRequest);
    }
}