namespace TripCraft.Application.Stops;

public interface IStopService
{
    /// <summary>Returns null if the trip doesn't exist or isn't owned by the user.</summary>
    Task<IReadOnlyList<StopDto>?> GetForTripAsync(Guid tripId, string userId, CancellationToken ct = default);

    /// <summary>Returns null if the trip doesn't exist or isn't owned by the user.</summary>
    Task<StopDto?> CreateAsync(Guid tripId, string userId, CreateStopRequest request, CancellationToken ct = default);

    Task<bool> UpdateAsync(Guid stopId, string userId, UpdateStopRequest request, CancellationToken ct = default);
    Task<bool> DeleteAsync(Guid stopId, string userId, CancellationToken ct = default);
}