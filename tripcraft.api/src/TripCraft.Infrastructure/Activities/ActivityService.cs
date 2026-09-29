using Microsoft.EntityFrameworkCore;
using TripCraft.Application.Activities;
using TripCraft.Domain.Entities;
using TripCraft.Infrastructure.Persistence;

namespace TripCraft.Infrastructure.Activities;

public class ActivityService : IActivityService
{
    private readonly AppDbContext _db;
    public ActivityService(AppDbContext db) => _db = db;

    // TimeOnly -> "HH:mm" formatting happens in memory (not translatable to SQL), so we map after loading.
    private static ActivityDto ToDto(Activity a) =>
        new(a.Id, a.StopId, a.Title, a.Notes, a.Day, a.TimeSlot?.ToString("HH:mm"));

    private static TimeOnly? ParseTime(string? value) =>
        !string.IsNullOrWhiteSpace(value) && TimeOnly.TryParse(value, out var t) ? t : null;

    private Task<bool> StopIsOwnedAsync(Guid stopId, string userId, CancellationToken ct) =>
        _db.Stops.AnyAsync(s => s.Id == stopId && s.Trip!.UserId == userId, ct);

    public async Task<IReadOnlyList<ActivityDto>?> GetForStopAsync(Guid stopId, string userId, CancellationToken ct = default)
    {
        if (!await StopIsOwnedAsync(stopId, userId, ct)) return null;

        var activities = await _db.Activities
            .AsNoTracking()
            .Where(a => a.StopId == stopId)
            .OrderBy(a => a.Day)
            .ThenBy(a => a.TimeSlot)
            .ToListAsync(ct);

        return activities.Select(ToDto).ToList();
    }

    public async Task<ActivityDto?> CreateAsync(Guid stopId, string userId, CreateActivityRequest request, CancellationToken ct = default)
    {
        if (!await StopIsOwnedAsync(stopId, userId, ct)) return null;

        var activity = new Activity
        {
            StopId = stopId,
            Title = request.Title.Trim(),
            Notes = request.Notes?.Trim(),
            Day = request.Day,
            TimeSlot = ParseTime(request.TimeSlot)
        };

        _db.Activities.Add(activity);
        await _db.SaveChangesAsync(ct);

        return ToDto(activity);
    }

    public async Task<bool> UpdateAsync(Guid activityId, string userId, UpdateActivityRequest request, CancellationToken ct = default)
    {
        // ownership chain: Activity -> Stop -> Trip -> UserId
        var activity = await _db.Activities
            .FirstOrDefaultAsync(a => a.Id == activityId && a.Stop!.Trip!.UserId == userId, ct);
        if (activity is null) return false;

        activity.Title = request.Title.Trim();
        activity.Notes = request.Notes?.Trim();
        activity.Day = request.Day;
        activity.TimeSlot = ParseTime(request.TimeSlot);

        await _db.SaveChangesAsync(ct);
        return true;
    }

    public async Task<bool> DeleteAsync(Guid activityId, string userId, CancellationToken ct = default)
    {
        var activity = await _db.Activities
            .FirstOrDefaultAsync(a => a.Id == activityId && a.Stop!.Trip!.UserId == userId, ct);
        if (activity is null) return false;

        _db.Activities.Remove(activity);
        await _db.SaveChangesAsync(ct);
        return true;
    }
}