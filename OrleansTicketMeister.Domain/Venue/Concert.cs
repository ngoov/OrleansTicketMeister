namespace OrleansTicketMeister.Domain.Venue;

public sealed class Concert
{
    public Guid Id { get; }
    public string Description { get; }
    public DateTime Date { get; }
    public Venue Venue { get; } = null!;

    private Concert(Guid id)
    {
        Id = id;
        Description = default!;
    }
    public Concert(Guid id, string description, DateTime date) : this(id)
    {
        Description = description;
        Date = date;
    }
}