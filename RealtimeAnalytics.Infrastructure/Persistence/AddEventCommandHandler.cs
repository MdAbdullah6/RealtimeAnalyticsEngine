using System.Data;
using Dapper;
using MediatR;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using RealtimeAnalytics.Application.Features.Analytics.Commands;

namespace RealtimeAnalytics.Infrastructure.Persistence;

public class AddEventCommandHandler : IRequestHandler<AddEventCommand, int>
{
    private readonly string _connectionString;

    public AddEventCommandHandler(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")
                            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' is missing.");
    }

    public async Task<int> Handle(AddEventCommand request, CancellationToken cancellationToken)
    {
        using IDbConnection db = new SqlConnection(_connectionString);

        const string sql = @"INSERT INTO AnalyticsEvents (Amount)
                             OUTPUT INSERTED.Id
                             VALUES (@Amount)";

        return await db.ExecuteScalarAsync<int>(sql, new { request.Amount });
    }
}