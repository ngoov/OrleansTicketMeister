using Orleans;
using Orleans.Runtime;
using OrleansTicketMeister.Contracts;
using OrleansTicketMeister.Domain;

namespace OrleansTicketMeister.Silo.Concerts;

internal class ConcertGrain : Grain, IConcertGrain
{
    private readonly IPersistentState<ConcertState> _state;
    private readonly IConcertSeatsService _concertSeatsService;
    
    public ConcertGrain(
        // Inject some storage. We will use the "definitions" storage provider configured in Program.cs
        // and we will call this piece of state "concert", to distinguish it from any other state we might want to have
        [PersistentState(stateName: "concert", storageName: "definitions")]
        IPersistentState<ConcertState> concertState,
        IConcertSeatsService concertSeatsService)
    {
        _state = concertState;
        _concertSeatsService = concertSeatsService;
    }
    public override async Task OnActivateAsync()
    {
        if (_state.State?.SeatReservations is null)
        {
            // If no state exists yet, get the data from the venue service
            Guid concertId = this.GetPrimaryKey();
            ICollection<Seat> seatsForConcert = await _concertSeatsService.GetSeatsForConcert(concertId);
            _state.State = new ConcertState
            {
                SeatReservations = new List<SeatReservation>()
            };

            foreach (Seat seat in seatsForConcert)
            {
                _state.State.SeatReservations.Add(new SeatReservation(seat, User.Empty));
            }

            await _state.WriteStateAsync();
        }
    }
    public ValueTask<IReadOnlyCollection<SeatsReservationDto>> GetSeatsAsync() 
        => ValueTask.FromResult<IReadOnlyCollection<SeatsReservationDto>>(_state.State.SeatReservations.Select(x => new SeatsReservationDto
        {
            IsReserved = x.User != User.Empty,
            Seat = new SeatDto
            {
                RowNumber = x.Seat.RowNumber,
                SeatNumber = x.Seat.SeatNumber
            } 
        }).ToList());
    public async Task ReserveSeatAsync(Seat seat, User user)
    {
        SeatReservation? seatReservation = _state.State.SeatReservations.FirstOrDefault(x => x.Seat == seat);
        if (seatReservation is null)
        {
            throw new ArgumentException("Seat does not exist");
        }
        if (seatReservation.User != User.Empty)
        {
            throw new ArgumentException("Seat is already reserved by user " + user.UserName);
        }

        seatReservation.ReserveForUser(user);
        await _state.WriteStateAsync();
    }
}
