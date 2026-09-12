namespace TripCraft.Domain.Entities;

public class Activity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid StopId { get; set; }
    public Stop? Stop { get; set; }

    public required string Title { get; set; }
    public string? Notes { get; set; }
    public int Day { get; set; }
    public TimeOnly? TimeSlot { get; set; }
}