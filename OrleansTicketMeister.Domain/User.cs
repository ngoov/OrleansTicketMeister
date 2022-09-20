namespace OrleansTicketMeister.Domain;

[Serializable]
public class User
{
    public Guid Id { get; }
    public string UserName { get; }
    public User(Guid id, string userName)
    {
        Id = id;
        UserName = userName;

    }
    public static User Empty => new(Guid.Empty, string.Empty);
    
}
