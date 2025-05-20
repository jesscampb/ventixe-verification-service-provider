using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using VerificationServiceProvider.Data.Contexts;
using VerificationServiceProvider.Services;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

builder.Services.AddDbContext<VerificationDbContext>(x => x.UseSqlServer(Environment.GetEnvironmentVariable("SqlConnection")));
builder.Services.AddScoped<IVerificationService, VerificationService>();

builder.Services
    .AddApplicationInsightsTelemetryWorkerService()
    .ConfigureFunctionsApplicationInsights();

builder.Build().Run();
