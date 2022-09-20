namespace OrleansTicketMeister.Domain.Venue;

public interface IVenueRepository
{
    ValueTask<IReadOnlyCollection<Venue>> GetAllAsync(CancellationToken cancellationToken = default);
    ValueTask<Venue?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
}
