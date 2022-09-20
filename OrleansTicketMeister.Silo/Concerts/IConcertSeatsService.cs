using OrleansTicketMeister.Domain;

namespace OrleansTicketMeister.Silo.Concerts;

public interface IConcertSeatsService
{
    Task<ICollection<Seat>> GetSeatsForConcert(Guid id);
}
