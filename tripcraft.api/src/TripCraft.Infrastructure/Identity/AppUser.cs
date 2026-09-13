using Microsoft.AspNetCore.Identity;

namespace TripCraft.Infrastructure.Identity;

public class AppUser : IdentityUser
{
    public string? DisplayName { get; set; }
}