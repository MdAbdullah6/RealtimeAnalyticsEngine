using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using RealtimeAnalytics.Api.Hubs;
using RealtimeAnalytics.Application.Features.Analytics.Commands;
using RealtimeAnalytics.Application.Features.Analytics.Queries;

namespace RealtimeAnalytics.Api.Controllers;

public record AddEventRequest(decimal Amount);

[ApiController]
[Route("api/[controller]")]
public class AnalyticsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IHubContext<AnalyticsHub> _hub;

    public AnalyticsController(IMediator mediator, IHubContext<AnalyticsHub> hub)
    {
        _mediator = mediator;
        _hub = hub;
    }

    [HttpGet]
    public async Task<IActionResult> GetAnalytics()
    {
        var result = await _mediator.Send(new GetAnalyticsQuery());
        return Ok(result);
    }

    [HttpPost("events")]
    public async Task<IActionResult> AddEvent([FromBody] AddEventRequest request)
    {
        if (request.Amount <= 0)
            return BadRequest("Amount must be greater than 0.");

        await _mediator.Send(new AddEventCommand(request.Amount));

        // Get the new totals and push them to every open browser
        var summary = await _mediator.Send(new GetAnalyticsQuery());
        await _hub.Clients.All.SendAsync("ReceiveAnalytics", summary);

        return Ok(summary);
    }
}