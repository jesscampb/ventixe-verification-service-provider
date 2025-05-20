using System;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace VerificationServiceProvider.Functions;

public class RemoveExpiredVerificationCodes
{
    private readonly ILogger _logger;

    public RemoveExpiredVerificationCodes(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<RemoveExpiredVerificationCodes>();
    }

    [Function("RemoveExpiredVerificationCodes")]
    public void Run([TimerTrigger("0 0 0 * * *")] TimerInfo myTimer)
    {
        _logger.LogInformation("C# Timer trigger function executed at: {executionTime}", DateTime.Now);
        
        if (myTimer.ScheduleStatus is not null)
        {
            _logger.LogInformation("Next timer schedule at: {nextSchedule}", myTimer.ScheduleStatus.Next);
        }
    }
}