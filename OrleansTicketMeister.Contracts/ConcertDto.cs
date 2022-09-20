namespace OrleansTicketMeister.Contracts;

public sealed record ConcertDto
{
    public Guid Id { get; init; }
    public string Description { get; init; } = default!;
    public DateTime Date { get; init; }
}
