using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TripCraft.Application.Stops;
using TripCraft.Domain.Entities;
using TripCraft.Infrastructure.Persistence;

namespace TripCraft.Infrastructure.Stops;

public class StopService : IStopService
{
    private readonly AppDbContext _db;
    public StopService(AppDbContext db) => _db = db;

    // One projection reused by every query. EF translates it to a SELECT of just these columns.
    private static readonly Expression<Func<Stop, StopDto>> ToDto =
        s => new StopDto(s.Id, s.TripId, s.Name, s.Address, s.Latitude, s.Longitude, s.Order);

    private Task<bool> TripIsOwnedAsync(Guid tripId, string userId, CancellationToken ct) =>
        _db.Trips.AnyAsync(t => t.Id == tripId && t.UserId == userId, ct);

    public async Task<IReadOnlyList<StopDto>?> GetForTripAsync(Guid tripId, string userId, CancellationToken ct = default)
    {
        if (!await TripIsOwnedAsync(tripId, userId, ct)) return null;

        return await _db.Stops
            .AsNoTracking()
            .Where(s => s.TripId == tripId)
            .OrderBy(s => s.Order)
            .Select(ToDto)
            .ToListAsync(ct);
    }

    public async Task<StopDto?> CreateAsync(Guid tripId, string userId, CreateStopRequest request, CancellationToken ct = default)
    {
        if (!await TripIsOwnedAsync(tripId, userId, ct)) return null;

        var order = request.Order;
        if (order <= 0)
        {
            // append: highest existing order + 1 (MaxAsync on int? returns null for an empty trip)
            var currentMax = await _db.Stops
                .Where(s => s.TripId == tripId)
                .MaxAsync(s => (int?)s.Order, ct);
            order = (currentMax ?? 0) + 1;
        }

        var stop = new Stop
        {
            TripId = tripId,
            Name = request.Name.Trim(),
            Address = request.Address?.Trim(),
            Latitude = request.Latitude,
            Longitude = request.Longitude,
            Order = order
        };

        _db.Stops.Add(stop);
        await _db.SaveChangesAsync(ct);

        return new StopDto(stop.Id, stop.TripId, stop.Name, stop.Address, stop.Latitude, stop.Longitude, stop.Order);
    }

    public async Task<bool> UpdateAsync(Guid stopId, string userId, UpdateStopRequest request, CancellationToken ct = default)
    {
        // ownership goes through the parent trip: Stop -> Trip -> UserId
        var stop = await _db.Stops.FirstOrDefaultAsync(s => s.Id == stopId && s.Trip!.UserId == userId, ct);
        if (stop is null) return false;

        stop.Name = request.Name.Trim();
        stop.Address = request.Address?.Trim();
        stop.Latitude = request.Latitude;
        stop.Longitude = request.Longitude;
        if (request.Order > 0) stop.Order = request.Order;

        await _db.SaveChangesAsync(ct);
        return true;
    }

    public async Task<bool> DeleteAsync(Guid stopId, string userId, CancellationToken ct = default)
    {
        var stop = await _db.Stops.FirstOrDefaultAsync(s => s.Id == stopId && s.Trip!.UserId == userId, ct);
        if (stop is null) return false;

        _db.Stops.Remove(stop);                               // cascades to the stop's activities
        await _db.SaveChangesAsync(ct);
        return true;
    }
}