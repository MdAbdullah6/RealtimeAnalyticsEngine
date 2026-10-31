using Microsoft.AspNetCore.SignalR;

namespace RealtimeAnalytics.Api.Hubs;

public class AnalyticsHub : Hub
{
    public async Task SendMetricsUpdate(object metrics)
    {
        await Clients.All.SendAsync("ReceiveMetricsUpdate", metrics);
    }
}