using Orleans;
using OrleansTicketMeister.Contracts;

namespace OrleansTicketMeister.Silo.Venues;

public interface IVenueGrain : IGrainWithGuidKey
{
    Task<VenueConcertsDto> GetConcertsAsync();
}