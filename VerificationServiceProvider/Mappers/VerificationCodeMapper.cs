using VerificationServiceProvider.Data.Entities;
using VerificationServiceProvider.Models;

namespace VerificationServiceProvider.Mappers;

public static class VerificationCodeMapper
{
    public static VerificationCodeEntity ToEntity(this VerificationCodeModel model)
    {
        return new VerificationCodeEntity
        {
            Email = model.Email,
            Code = model.Code,
            CreatedAt = model.CreatedAt,
            ExpiresAt = model.ExpiresAt,
        };
    }
}
