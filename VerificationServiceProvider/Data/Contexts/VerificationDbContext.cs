using Microsoft.EntityFrameworkCore;

namespace VerificationServiceProvider.Data.Contexts;

public class VerificationDbContext(DbContextOptions<VerificationDbContext> options) : DbContext(options)
{
    
}
