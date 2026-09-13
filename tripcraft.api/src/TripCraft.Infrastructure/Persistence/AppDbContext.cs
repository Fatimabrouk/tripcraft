using Microsoft.EntityFrameworkCore;
using TripCraft.Domain.Entities;

namespace TripCraft.Infrastructure.Persistence;

public class AppDbContext : IdentityDbContext<AppUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Trip> Trips => Set<Trip>();
    public DbSet<Stop> Stops => Set<Stop>();
    public DbSet<Activity> Activities => Set<Activity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Trip>()
            .HasMany(t => t.Stops)
            .WithOne(s => s.Trip)
            .HasForeignKey(s => s.TripId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Stop>()
            .HasMany(s => s.Activities)
            .WithOne(a => a.Stop)
            .HasForeignKey(a => a.StopId)
            .OnDelete(DeleteBehavior.Cascade);

        base.OnModelCreating(modelBuilder);
    }
}