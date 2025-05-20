using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore.Storage.Json;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using VerificationServiceProvider.Models;
using VerificationServiceProvider.Services;

namespace VerificationServiceProvider.Functions;

public class ValidateVerificationCode(ILogger<ValidateVerificationCode> logger, IVerificationService verificationService)
{
    private readonly ILogger<ValidateVerificationCode> _logger = logger;
    private readonly IVerificationService _verificationService = verificationService;

    [Function("ValidateVerificationCode")]

    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequest req)
    {
        var body = await new StreamReader(req.Body).ReadToEndAsync();
        var validateRequest = JsonConvert.DeserializeObject<ValidateRequestModel>(body);

        var result = await _verificationService.ValidateVerificationCodeAsync(validateRequest);
        return result
            ? new OkObjectResult(new { message = "Validation succeeded." })
            : new UnauthorizedObjectResult(new { message = "Verification code invalid or expired." });
    }
}