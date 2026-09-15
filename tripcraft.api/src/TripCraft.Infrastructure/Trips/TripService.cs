using Microsoft.EntityFrameworkCore;
using TripCraft.Application.Trips;
using TripCraft.Domain.Entities;
using TripCraft.Infrastructure.Persistence;

namespace TripCraft.Infrastructure.Trips;

public class TripService : ITripService
{
    private readonly AppDbContext _db;
    public TripService(AppDbContext db) => _db = db;

    public async Task<IEnumerable<TripDto>> GetTripsForUserAsync(string userId, CancellationToken ct = default)
    {
        return await _db.Trips
            .Where(t => t.UserId == userId)
            .Select(t => new TripDto(t.Id, t.Title, t.Description, t.StartDate, t.EndDate, t.Stops.Count))
            .ToListAsync(ct);
    }

    public async Task<TripDto?> GetByIdAsync(Guid id, string userId, CancellationToken ct = default)
    {
        var t = await _db.Trips.Include(x => x.Stops)
            .FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId, ct);
        return t is null ? null : new TripDto(t.Id, t.Title, t.Description, t.StartDate, t.EndDate, t.Stops.Count);
    }

    public async Task<TripDto> CreateAsync(string userId, CreateTripRequest request, CancellationToken ct = default)
    {
        var trip = new Trip
        {
            UserId = userId,
            Title = request.Title,
            Description = request.Description,
            StartDate = request.StartDate,
            EndDate = request.EndDate
        };
        _db.Trips.Add(trip);
        await _db.SaveChangesAsync(ct);
        return new TripDto(trip.Id, trip.Title, trip.Description, trip.StartDate, trip.EndDate, 0);
    }

    public async Task<bool> UpdateAsync(Guid id, string userId, UpdateTripRequest request, CancellationToken ct = default)
    {
        var trip = await _db.Trips.FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId, ct);
        if (trip is null) return false;

        trip.Title = request.Title;
        trip.Description = request.Description;
        trip.StartDate = request.StartDate;
        trip.EndDate = request.EndDate;
        await _db.SaveChangesAsync(ct);
        return true;
    }

    public async Task<bool> DeleteAsync(Guid id, string userId, CancellationToken ct = default)
    {
        var trip = await _db.Trips.FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId, ct);
        if (trip is null) return false;

        _db.Trips.Remove(trip);
        await _db.SaveChangesAsync(ct);
        return true;
    }
}