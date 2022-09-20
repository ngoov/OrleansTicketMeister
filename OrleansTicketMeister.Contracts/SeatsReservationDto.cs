namespace OrleansTicketMeister.Contracts;

public sealed record SeatsReservationDto
{
    public bool IsReserved { get; init; }
    public SeatDto Seat { get; init; } = default!;
}

