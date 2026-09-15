namespace TripCraft.Application.Trips;

public interface ITripService
{
    Task<IEnumerable<TripDto>> GetTripsForUserAsync(string userId, CancellationToken ct = default);
    Task<TripDto?> GetByIdAsync(Guid id, string userId, CancellationToken ct = default);
    Task<TripDto> CreateAsync(string userId, CreateTripRequest request, CancellationToken ct = default);
    Task<bool> UpdateAsync(Guid id, string userId, UpdateTripRequest request, CancellationToken ct = default);
    Task<bool> DeleteAsync(Guid id, string userId, CancellationToken ct = default);
}