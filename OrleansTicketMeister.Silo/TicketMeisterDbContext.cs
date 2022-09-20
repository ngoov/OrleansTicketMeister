using Microsoft.EntityFrameworkCore;
using OrleansTicketMeister.Domain.Venue;

namespace OrleansTicketMeister.Silo;

public class TicketMeisterDbContext : DbContext
{
    public TicketMeisterDbContext(DbContextOptions<TicketMeisterDbContext> options) : base(options) { }
    public DbSet<Venue> Venues => Set<Venue>();
    public DbSet<Concert> Concerts => Set<Concert>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Venue>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Id);
            e.Property(x => x.Name);
            e.HasMany(x => x.Concerts).WithOne(x => x.Venue).HasForeignKey("VenueId");
        });
        modelBuilder.Entity<Concert>(e =>
        {
            e.ToTable("Concert");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id);
            e.Property(x => x.Date);
            e.Property(x => x.Description);
        });
    }
}
