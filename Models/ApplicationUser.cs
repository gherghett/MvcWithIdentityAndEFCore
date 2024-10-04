using Microsoft.AspNetCore.Identity;
namespace MvcWithIdentityAndEFCore.Models;

public class ApplicationUser : IdentityUser
{
    // Extra egenskaper för din applikation
    public ICollection<Event> Events { get; set; } = new List<Event>();
}
