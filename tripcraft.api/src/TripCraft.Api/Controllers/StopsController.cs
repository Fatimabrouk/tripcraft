using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using TripCraft.Application.Stops;

namespace TripCraft.Api.Controllers;

[Route("api")]
public class StopsController : ApiControllerBase
{
    private readonly IStopService _stops;
    private readonly IValidator<CreateStopRequest> _createValidator;
    private readonly IValidator<UpdateStopRequest> _updateValidator;

    public StopsController(
        IStopService stops,
        IValidator<CreateStopRequest> createValidator,
        IValidator<UpdateStopRequest> updateValidator)
    {
        _stops = stops;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    [HttpGet("trips/{tripId:guid}/stops")]
    public async Task<ActionResult<IReadOnlyList<StopDto>>> GetForTrip(Guid tripId, CancellationToken ct)
    {
        var stops = await _stops.GetForTripAsync(tripId, UserId, ct);
        return stops is null ? NotFound() : Ok(stops);
    }

    [HttpPost("trips/{tripId:guid}/stops")]
    public async Task<ActionResult<StopDto>> Create(Guid tripId, CreateStopRequest request, CancellationToken ct)
    {
        if (await ValidateAsync(_createValidator, request, ct) is { } invalid) return invalid;

        var stop = await _stops.CreateAsync(tripId, UserId, request, ct);
        return stop is null ? NotFound() : Created($"/api/stops/{stop.Id}", stop);
    }

    [HttpPut("stops/{id:guid}")]
    public async Task<IActionResult> Update(Guid id, UpdateStopRequest request, CancellationToken ct)
    {
        if (await ValidateAsync(_updateValidator, request, ct) is { } invalid) return invalid;

        return await _stops.UpdateAsync(id, UserId, request, ct) ? NoContent() : NotFound();
    }

    [HttpDelete("stops/{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
        => await _stops.DeleteAsync(id, UserId, ct) ? NoContent() : NotFound();
}