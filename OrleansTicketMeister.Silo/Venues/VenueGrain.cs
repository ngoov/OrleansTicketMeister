using Orleans;
using OrleansTicketMeister.Contracts;
using OrleansTicketMeister.Domain.Venue;

namespace OrleansTicketMeister.Silo.Venues;

public class VenueGrain : Grain, IVenueGrain
{
    private readonly IVenueRepository _venueRepository;
    private Venue _venue = null!;
    public VenueGrain(IVenueRepository venueRepository)
    {
        _venueRepository = venueRepository;
    }
    public override async Task OnActivateAsync()
    {
        Guid id = this.GetPrimaryKey();
        Venue? venue = await _venueRepository.GetByIdAsync(id);
        _venue = venue ?? throw new NullReferenceException($"Venue with id {this.GetPrimaryKey()} does not exist");
    }

    public Task<VenueConcertsDto> GetConcertsAsync()
    {
        return Task.FromResult(new VenueConcertsDto
        {
            VenueName = _venue.Name,
            Concerts = _venue.Concerts.Select(x => new ConcertDto
            {
                Id = x .Id,
                Date = x.Date,
                Description = x.Description
            }).ToList()
        });
    }
}
