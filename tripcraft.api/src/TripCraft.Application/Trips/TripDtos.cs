namespace TripCraft.Application.Trips;

/// <summary>What the API returns. StopCount is computed, not stored.</summary>
public record TripDto(
    Guid Id,
    string Title,
    string? Description,
    DateOnly StartDate,
    DateOnly EndDate,
    int StopCount);

public record CreateTripRequest(
    string Title,
    string? Description,
    DateOnly StartDate,
    DateOnly EndDate);

public record UpdateTripRequest(
    string Title,
    string? Description,
    DateOnly StartDate,
    DateOnly EndDate);