using Microsoft.EntityFrameworkCore;
using VerificationServiceProvider.Data.Entities;

namespace VerificationServiceProvider.Data.Contexts;

public class VerificationDbContext(DbContextOptions<VerificationDbContext> options) : DbContext(options)
{
    public DbSet<VerificationCodeEntity> VerificationCodes { get; set; }
}
