using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace VerificationServiceProvider.Data.Contexts;

public class VerificationDbContextFactory : IDesignTimeDbContextFactory<VerificationDbContext>
{
    public VerificationDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("local.settings.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        var connectionString = configuration["Values:SqlConnection"];
        var optionsBuilder = new DbContextOptionsBuilder<VerificationDbContext>();
        optionsBuilder.UseSqlServer(connectionString);

        return new VerificationDbContext(optionsBuilder.Options);
    }
}
