using Hangfire;
using Hangfire.MemoryStorage;
using RealtimeAnalytics.Api.Hubs;
using RealtimeAnalytics.Application.Features.Analytics.Queries;
using RealtimeAnalytics.Infrastructure.Jobs;

var builder = WebApplication.CreateBuilder(args);

string connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? "Server=.;Database=AnalyticsDb;Trusted_Connection=True;TrustServerCertificate=True;";

// Register MediatR for both Application and Infrastructure assemblies
builder.Services.AddMediatR(cfg => {
    cfg.RegisterServicesFromAssemblyContaining<GetAnalyticsQuery>();
    cfg.RegisterServicesFromAssembly(typeof(RealtimeAnalytics.Infrastructure.Persistence.GetAnalyticsQueryHandler).Assembly);
});

// 2. Register SignalR
builder.Services.AddSignalR();

// 3. Register Hangfire with Memory Storage (No SQL Server required)
builder.Services.AddHangfire(config => config
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UseMemoryStorage());
builder.Services.AddHangfireServer();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseDefaultFiles();
app.UseStaticFiles();
app.UseAuthorization();

// Hangfire Dashboard Endpoint
app.UseHangfireDashboard("/hangfire");

// Map SignalR Hub Endpoint
app.MapHub<AnalyticsHub>("/hubs/analytics");

app.MapControllers();

// Schedule Recurring Background Job via Hangfire
RecurringJob.AddOrUpdate<AnalyticsReportJob>(
    "daily-analytics-job",
    job => job.ProcessDailyAnalyticsAsync(),
    Cron.Daily);

app.Run();