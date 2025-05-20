using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;
using VerificationServiceProvider.Models;
using VerificationServiceProvider.Services;

namespace VerificationServiceProvider.Functions;

public class ValidateVerificationCode(ILogger<ValidateVerificationCode> logger, IVerificationService verificationService)
{
    private readonly ILogger<ValidateVerificationCode> _logger = logger;
    private readonly IVerificationService _verificationService = verificationService;

    [Function("ValidateVerificationCode")]

    public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = "verify-code")] HttpRequestData request)
    {
        string json = await new StreamReader(request.Body).ReadToEndAsync();
        ValidateRequestModel? payload;

        try
        {
            payload = JsonSerializer.Deserialize<ValidateRequestModel>(json);
        }
        catch (JsonException)
        {
            _logger.LogWarning("Invalid JSON in validation request");

            var badJson = request.CreateResponse(HttpStatusCode.BadRequest);
            await badJson.WriteStringAsync("Invalid JSON payload");
            return badJson;
        }

        if (payload == null
            || string.IsNullOrWhiteSpace(payload.Email)
            || string.IsNullOrWhiteSpace(payload.Code))
        {
            _logger.LogWarning("Missing email or verification code in request.");

            var bad = request.CreateResponse(HttpStatusCode.BadRequest);
            await bad.WriteStringAsync("Email and verification code must be provided.");
            return bad;
        }

        bool isValid = await _verificationService.ValidateVerificationCodeAsync(payload);

        if (!isValid)
        {
            _logger.LogInformation("Verification failed or expired for {Email}", payload.Email);

            var fail = request.CreateResponse(HttpStatusCode.BadRequest);
            await fail.WriteStringAsync("Invalid or expired verification code.");
            return fail;
        }

        _logger.LogInformation("Verification succeeded for {Email}", payload.Email);

        var ok = request.CreateResponse(HttpStatusCode.OK);
        await ok.WriteStringAsync("Verification successful.");
        return ok;
    }
}