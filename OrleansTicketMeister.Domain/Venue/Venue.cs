namespace OrleansTicketMeister.Domain.Venue;

public sealed class Venue
{
    private readonly ICollection<Concert> _concerts = new List<Concert>();
    public IReadOnlyCollection<Concert> Concerts => _concerts.ToList();
    public Guid Id { get; }
    public string Name { get; }
    private Venue(Guid id)
    {
        Id = id;
        Name = null!;
    }
    public Venue(Guid id, string name) : this(id)
    {
        Name = name;
    }
    
    public void RegisterConcert(Concert concert) 
        => _concerts.Add(concert);
    
    // Other domain behavior
}
