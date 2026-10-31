using MediatR;
using RealtimeAnalytics.Application.DTOs;

namespace RealtimeAnalytics.Application.Features.Analytics.Queries;

public record GetAnalyticsQuery() : IRequest<AnalyticsSummaryDto>;