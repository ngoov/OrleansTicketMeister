using Orleans;
using OrleansTicketMeister.Contracts;
using OrleansTicketMeister.Domain;

namespace OrleansTicketMeister.Silo.Concerts;

public interface IConcertGrain : IGrainWithGuidKey
{
    ValueTask<IReadOnlyCollection<SeatsReservationDto>> GetSeatsAsync();
    Task ReserveSeatAsync(Seat seat, User user);
}
