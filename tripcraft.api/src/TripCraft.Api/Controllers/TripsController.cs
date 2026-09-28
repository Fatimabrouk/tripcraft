using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using TripCraft.Application.Trips;

namespace TripCraft.Api.Controllers;

[Route("api/trips")]
public class TripsController : ApiControllerBase
{
    private readonly ITripService _trips;
    private readonly IValidator<CreateTripRequest> _createValidator;
    private readonly IValidator<UpdateTripRequest> _updateValidator;

    public TripsController(
        ITripService trips,
        IValidator<CreateTripRequest> createValidator,
        IValidator<UpdateTripRequest> updateValidator)
    {
        _trips = trips;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<TripDto>>> GetAll(CancellationToken ct)
        => Ok(await _trips.GetTripsForUserAsync(UserId, ct));

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<TripDto>> GetById(Guid id, CancellationToken ct)
    {
        var trip = await _trips.GetByIdAsync(id, UserId, ct);
        return trip is null ? NotFound() : Ok(trip);
    }

    [HttpPost]
    public async Task<ActionResult<TripDto>> Create(CreateTripRequest request, CancellationToken ct)
    {
        if (await ValidateAsync(_createValidator, request, ct) is { } invalid) return invalid;

        var trip = await _trips.CreateAsync(UserId, request, ct);
        return CreatedAtAction(nameof(GetById), new { id = trip.Id }, trip);   // 201 + Location header
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, UpdateTripRequest request, CancellationToken ct)
    {
        if (await ValidateAsync(_updateValidator, request, ct) is { } invalid) return invalid;

        return await _trips.UpdateAsync(id, UserId, request, ct) ? NoContent() : NotFound();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
        => await _trips.DeleteAsync(id, UserId, ct) ? NoContent() : NotFound();
}