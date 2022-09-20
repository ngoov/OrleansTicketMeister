namespace OrleansTicketMeister.Contracts;

public sealed record VenueConcertsDto
{
    public string VenueName { get; init; }
    public ICollection<ConcertDto> Concerts { get; init; }
}
