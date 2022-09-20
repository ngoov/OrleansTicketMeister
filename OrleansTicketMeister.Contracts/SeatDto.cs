namespace OrleansTicketMeister.Contracts;

public sealed record SeatDto
{
    public int RowNumber { get; init; }
    public int SeatNumber { get; init; }
}
