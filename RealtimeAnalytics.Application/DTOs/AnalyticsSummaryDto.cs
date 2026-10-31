namespace RealtimeAnalytics.Application.DTOs;

public class AnalyticsSummaryDto
{
    public int TotalEvents { get; set; }
    public decimal TotalRevenue { get; set; }
    public DateTime LastUpdated { get; set; }
}