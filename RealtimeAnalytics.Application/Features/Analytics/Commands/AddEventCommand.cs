using MediatR;

namespace RealtimeAnalytics.Application.Features.Analytics.Commands;

public record AddEventCommand(decimal Amount) : IRequest<int>;