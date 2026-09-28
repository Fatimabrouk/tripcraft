namespace TripCraft.Application.Stops;

public record StopDto(
    Guid Id,
    Guid TripId,
    string Name,
    string? Address,
    double Latitude,
    double Longitude,
    int Order);

/// <summary>Order = 0 means "append to the end of the trip".</summary>
public record CreateStopRequest(
    string Name,
    string? Address,
    double Latitude,
    double Longitude,
    int Order);

public record UpdateStopRequest(
    string Name,
    string? Address,
    double Latitude,
    double Longitude,
    int Order);