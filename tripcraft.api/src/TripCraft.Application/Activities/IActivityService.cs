namespace TripCraft.Application.Activities;

public interface IActivityService
{
    /// <summary>Returns null if the stop doesn't exist or its trip isn't owned by the user.</summary>
    Task<IReadOnlyList<ActivityDto>?> GetForStopAsync(Guid stopId, string userId, CancellationToken ct = default);

    /// <summary>Returns null if the stop doesn't exist or its trip isn't owned by the user.</summary>
    Task<ActivityDto?> CreateAsync(Guid stopId, string userId, CreateActivityRequest request, CancellationToken ct = default);

    Task<bool> UpdateAsync(Guid activityId, string userId, UpdateActivityRequest request, CancellationToken ct = default);
    Task<bool> DeleteAsync(Guid activityId, string userId, CancellationToken ct = default);
}