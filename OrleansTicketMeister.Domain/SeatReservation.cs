namespace OrleansTicketMeister.Domain;

public class SeatReservation
{
    public Seat Seat { get; }
    public User User { get; private set; }
    public SeatReservation(Seat seat, User user)
    {
        Seat = seat;
        User = user;
    }
    public void ReserveForUser(User user)
    {
        User = user;
    }
}
