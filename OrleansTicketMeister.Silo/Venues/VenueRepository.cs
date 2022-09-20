using Microsoft.EntityFrameworkCore;
using OrleansTicketMeister.Domain.Venue;

namespace OrleansTicketMeister.Silo.Venues;

public class VenueRepository : IVenueRepository
{
    private readonly IDbContextFactory<TicketMeisterDbContext> _dbContextFactory;
    public VenueRepository(IDbContextFactory<TicketMeisterDbContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;

    }
    public async ValueTask<IReadOnlyCollection<Venue>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        await using TicketMeisterDbContext ticketMeisterDbContext = await _dbContextFactory.CreateDbContextAsync(cancellationToken);
        return await ticketMeisterDbContext.Venues.AsNoTracking().ToListAsync(cancellationToken);
    }

    public async ValueTask<Venue?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await using TicketMeisterDbContext ticketMeisterDbContext = await _dbContextFactory.CreateDbContextAsync(cancellationToken);
        return await ticketMeisterDbContext.Venues.AsNoTracking().Include(x => x.Concerts).FirstOrDefaultAsync(x => x.Id.Equals(id) , cancellationToken: cancellationToken);
    }
}
