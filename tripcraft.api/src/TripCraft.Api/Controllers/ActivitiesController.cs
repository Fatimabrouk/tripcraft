using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using TripCraft.Application.Activities;

namespace TripCraft.Api.Controllers;

[Route("api")]
public class ActivitiesController : ApiControllerBase
{
    private readonly IActivityService _activities;
    private readonly IValidator<CreateActivityRequest> _createValidator;
    private readonly IValidator<UpdateActivityRequest> _updateValidator;

    public ActivitiesController(
        IActivityService activities,
        IValidator<CreateActivityRequest> createValidator,
        IValidator<UpdateActivityRequest> updateValidator)
    {
        _activities = activities;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    [HttpGet("stops/{stopId:guid}/activities")]
    public async Task<ActionResult<IReadOnlyList<ActivityDto>>> GetForStop(Guid stopId, CancellationToken ct)
    {
        var activities = await _activities.GetForStopAsync(stopId, UserId, ct);
        return activities is null ? NotFound() : Ok(activities);
    }

    [HttpPost("stops/{stopId:guid}/activities")]
    public async Task<ActionResult<ActivityDto>> Create(Guid stopId, CreateActivityRequest request, CancellationToken ct)
    {
        if (await ValidateAsync(_createValidator, request, ct) is { } invalid) return invalid;

        var activity = await _activities.CreateAsync(stopId, UserId, request, ct);
        return activity is null ? NotFound() : Created($"/api/activities/{activity.Id}", activity);
    }

    [HttpPut("activities/{id:guid}")]
    public async Task<IActionResult> Update(Guid id, UpdateActivityRequest request, CancellationToken ct)
    {
        if (await ValidateAsync(_updateValidator, request, ct) is { } invalid) return invalid;

        return await _activities.UpdateAsync(id, UserId, request, ct) ? NoContent() : NotFound();
    }

    [HttpDelete("activities/{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
        => await _activities.DeleteAsync(id, UserId, ct) ? NoContent() : NotFound();
}