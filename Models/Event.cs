namespace MvcWithIdentityAndEFCore.Models;

public class Event
{
    public string Id { get; set;} = null!;
    public string Name { get; set;} = null!;
    // Navigeringsegenskap för många-till-många-relation
    public ICollection<ApplicationUser> EventParticipants { get; set; } = null!;
}
