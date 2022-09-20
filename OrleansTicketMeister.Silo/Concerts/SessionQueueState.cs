namespace OrleansTicketMeister.Silo.Concerts;

public sealed class SessionQueueState
{
    public SessionQueueState(DateTime enteredQueueAt)
    {
        EnteredQueueAt = enteredQueueAt;
    }
    public DateTime EnteredQueueAt { get; }
    public DateTime StartedReservingAt { get; } = DateTime.MinValue;
    public bool IsReserving => StartedReservingAt != DateTime.MinValue;
}
