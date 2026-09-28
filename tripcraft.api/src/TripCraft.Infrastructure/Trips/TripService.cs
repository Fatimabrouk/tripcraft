using Microsoft.EntityFrameworkCore;
using TripCraft.Application.Trips;
using TripCraft.Domain.Entities;
using TripCraft.Infrastructure.Persistence;

namespace TripCraft.Infrastructure.Trips;

public class TripService : ITripService
{
    private readonly AppDbContext _db;
    public TripService(AppDbContext db) => _db = db;

    public async Task<IReadOnlyList<TripDto>> GetTripsForUserAsync(string userId, CancellationToken ct = default)
    {
        return await _db.Trips
            .AsNoTracking()                                   // read-only: skips change tracking, faster
            .Where(t => t.UserId == userId)
            .OrderBy(t => t.StartDate)
            .Select(t => new TripDto(t.Id, t.Title, t.Description, t.StartDate, t.EndDate, t.Stops.Count))
            .ToListAsync(ct);                                 // Stops.Count becomes a SQL COUNT, stops aren't loaded
    }

    public async Task<TripDto?> GetByIdAsync(Guid id, string userId, CancellationToken ct = default)
    {
        return await _db.Trips
            .AsNoTracking()
            .Where(t => t.Id == id && t.UserId == userId)
            .Select(t => new TripDto(t.Id, t.Title, t.Description, t.StartDate, t.EndDate, t.Stops.Count))
            .FirstOrDefaultAsync(ct);
    }

    public async Task<TripDto> CreateAsync(string userId, CreateTripRequest request, CancellationToken ct = default)
    {
        var trip = new Trip
        {
            UserId = userId,                                  // from the JWT, never from the client
            Title = request.Title.Trim(),
            Description = request.Description?.Trim(),
            StartDate = request.StartDate,
            EndDate = request.EndDate
        };

        _db.Trips.Add(trip);
        await _db.SaveChangesAsync(ct);

        return new TripDto(trip.Id, trip.Title, trip.Description, trip.StartDate, trip.EndDate, 0);
    }

    public async Task<bool> UpdateAsync(Guid id, string userId, UpdateTripRequest request, CancellationToken ct = default)
    {
        var trip = await _db.Trips.FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId, ct);
        if (trip is null) return false;

        trip.Title = request.Title.Trim();
        trip.Description = request.Description?.Trim();
        trip.StartDate = request.StartDate;
        trip.EndDate = request.EndDate;

        await _db.SaveChangesAsync(ct);
        return true;
    }

    public async Task<bool> DeleteAsync(Guid id, string userId, CancellationToken ct = default)
    {
        var trip = await _db.Trips.FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId, ct);
        if (trip is null) return false;

        _db.Trips.Remove(trip);                               // cascade delete (configured in AppDbContext) removes stops and activities
        await _db.SaveChangesAsync(ct);
        return true;
    }
}