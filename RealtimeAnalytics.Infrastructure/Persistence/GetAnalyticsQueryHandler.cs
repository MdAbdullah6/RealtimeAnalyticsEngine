using System.Data;
using Dapper;
using MediatR;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using RealtimeAnalytics.Application.DTOs;
using RealtimeAnalytics.Application.Features.Analytics.Queries;

namespace RealtimeAnalytics.Infrastructure.Persistence;

public class GetAnalyticsQueryHandler : IRequestHandler<GetAnalyticsQuery, AnalyticsSummaryDto>
{
    private readonly string _connectionString;

    public GetAnalyticsQueryHandler(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")
                            ?? "Server=.;Database=AnalyticsDb;Trusted_Connection=True;TrustServerCertificate=True;";
    }

    public async Task<AnalyticsSummaryDto> Handle(GetAnalyticsQuery request, CancellationToken cancellationToken)
    {
        using IDbConnection db = new SqlConnection(_connectionString);

        // High-performance Dapper execution using raw SQL
        string sql = @"SELECT 
                        ISNULL(COUNT(Id), 0) AS TotalEvents, 
                        ISNULL(SUM(Amount), 0) AS TotalRevenue, 
                        GETDATE() AS LastUpdated 
                       FROM AnalyticsEvents (NOLOCK)";

        var result = await db.QueryFirstOrDefaultAsync<AnalyticsSummaryDto>(sql);
        return result ?? new AnalyticsSummaryDto();
    }
}