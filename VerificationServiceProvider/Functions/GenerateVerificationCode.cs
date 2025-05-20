using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using VerificationServiceProvider.Services;

namespace VerificationServiceProvider.Functions;

public class GenerateVerificationCode(ILogger<GenerateVerificationCode> logger, IVerificationService verificationService)
{
    private readonly ILogger<GenerateVerificationCode> _logger = logger;
    private readonly IVerificationService _verificationService = verificationService;

    [Function(nameof(GenerateVerificationCode))]
    [ServiceBusOutput("verification-email-requested", Connection = "ServiceBusConnection")]

    public async Task<string?> Run(
        [ServiceBusTrigger("verification-code-requested", Connection = "ServiceBusConnection")]
        ServiceBusReceivedMessage message,
        ServiceBusMessageActions messageActions)
    {
        var email = message.Body?.ToString();
        if (!string.IsNullOrEmpty(email))
        {
            var emailRequest = await _verificationService.GenerateVerificationCodeEmailAsync(email);
            if (emailRequest != null)
            {
                await messageActions.CompleteMessageAsync(message);
                _logger.LogInformation("Verification code was successfully sent.");

                return JsonSerializer.Serialize(emailRequest);
            }
        }

        _logger.LogWarning("Received null or empty email from message.");
        await messageActions.DeadLetterMessageAsync(message, new Dictionary<string, object> {
            { "Reason", "Email address is null or empty." }
        });

        return null;
    }
}