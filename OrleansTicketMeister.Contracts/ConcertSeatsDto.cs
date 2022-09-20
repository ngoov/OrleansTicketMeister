namespace OrleansTicketMeister.Contracts;

public sealed record ConcertSeatsDto
{
    public IReadOnlyCollection<SeatsReservationDto> SeatsReservations { get; init; } = default!;
    public string ConcertName { get; init; } = default!;
    public string VenueName { get; init; } = default!;
}
