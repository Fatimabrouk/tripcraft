using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TripCraft.Application.Trips;

namespace TripCraft.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/trips")]
public class TripsController : ControllerBase
{
    private readonly ITripService _tripService;
    public TripsController(ITripService tripService) => _tripService = tripService;

    private string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier)
        ?? throw new UnauthorizedAccessException();

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
        => Ok(await _tripService.GetTripsForUserAsync(UserId, ct));

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var trip = await _tripService.GetByIdAsync(id, UserId, ct);
        return trip is null ? NotFound() : Ok(trip);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateTripRequest request, CancellationToken ct)
    {
        var trip = await _tripService.CreateAsync(UserId, request, ct);
        return CreatedAtAction(nameof(GetById), new { id = trip.Id }, trip);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, UpdateTripRequest request, CancellationToken ct)
        => await _tripService.UpdateAsync(id, UserId, request, ct) ? NoContent() : NotFound();

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
        => await _tripService.DeleteAsync(id, UserId, ct) ? NoContent() : NotFound();
}