namespace TripCraft.Application.Activities;

/// <summary>TimeSlot is "HH:mm" (e.g. "09:30") or null.</summary>
public record ActivityDto(
    Guid Id,
    Guid StopId,
    string Title,
    string? Notes,
    int Day,
    string? TimeSlot);

public record CreateActivityRequest(
    string Title,
    string? Notes,
    int Day,
    string? TimeSlot);

public record UpdateActivityRequest(
    string Title,
    string? Notes,
    int Day,
    string? TimeSlot);