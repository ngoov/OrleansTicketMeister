using OrleansTicketMeister.Domain;

namespace OrleansTicketMeister.Silo.Concerts;

[Serializable]
internal class ConcertState
{
    public ICollection<SeatReservation> SeatReservations { get; set; } = default!;
}