namespace TripCraft.Application.Trips;

public record TripDto(Guid Id, string Title, string? Description, DateOnly StartDate, DateOnly EndDate, int StopCount);
public record CreateTripRequest(string Title, string? Description, DateOnly StartDate, DateOnly EndDate);
public record UpdateTripRequest(string Title, string? Description, DateOnly StartDate, DateOnly EndDate);