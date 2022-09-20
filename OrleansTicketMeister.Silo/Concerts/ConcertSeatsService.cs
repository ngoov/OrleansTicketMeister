using OrleansTicketMeister.Domain;

namespace OrleansTicketMeister.Silo.Concerts;

public class ConcertSeatsService : IConcertSeatsService
{
    public Task<ICollection<Seat>> GetSeatsForConcert(Guid id)
        => Task.FromResult<ICollection<Seat>>(RandomSeatsGenerator().ToList());
    private IEnumerable<Seat> RandomSeatsGenerator()
    {
        var random = new Random();
        int rowCount = random.Next(1, 11);
        for (var rowNumber = 0; rowNumber < rowCount; rowNumber++)
        {
            int seatsCount = random.Next(10, 31);
            for (var seatNumber = 0; seatNumber < seatsCount; seatNumber++)
            {
                yield return new Seat(rowNumber, seatNumber);
            }
        }
    }
}
