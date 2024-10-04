using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MvcWithIdentityAndEFCore.Models;
namespace MvcWithIdentityAndEFCore.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    // DbSet för din applikationsdata
    public DbSet<Event> Events { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Event>()
            .HasMany(e => e.EventParticipants)
            .WithMany(u => u.Events)
            .UsingEntity(j => j.ToTable("EventParticipants"));
    }
}
