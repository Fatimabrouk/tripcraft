namespace TripCraft.Domain.Entities;

public class Stop
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid TripId { get; set; }
    public Trip? Trip { get; set; }

    public required string Name { get; set; }
    public string? Address { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public int Order { get; set; }

    public ICollection<Activity> Activities { get; set; } = new List<Activity>();
}