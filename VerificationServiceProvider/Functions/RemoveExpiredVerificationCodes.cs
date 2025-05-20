using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using VerificationServiceProvider.Data.Contexts;

namespace VerificationServiceProvider.Functions;

public class RemoveExpiredVerificationCodes(ILogger<RemoveExpiredVerificationCodes> logger, VerificationDbContext context)
{
    private readonly ILogger<RemoveExpiredVerificationCodes> _logger = logger;
    private readonly VerificationDbContext _context = context;

    [Function("RemoveExpiredVerificationCodes")]

    // Körs varje midnatt
    public async Task Run([TimerTrigger("0 0 0 * * *", RunOnStartup = false)] TimerInfo timer)
    {
        var now = DateTime.UtcNow;

        _logger.LogInformation("RemoveExpiredVerificationCodes triggered at UTC {Time}", now);

        var expired = await _context.VerificationCodes
            .Where(x => x.ExpiresAt <= now)
            .ToListAsync();

        if (!expired.Any())
        {
            _logger.LogInformation("No expired verification codes to remove.");
            return;
        }

        _context.VerificationCodes.RemoveRange(expired);
        var count = await _context.SaveChangesAsync();

        _logger.LogInformation("Removed {Count} expired verification codes.", count);
    }
}