namespace OrleansTicketMeister.Contracts;

public sealed record VenueDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = default!;
}