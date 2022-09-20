using MediatR;
using Microsoft.EntityFrameworkCore;
using OrleansTicketMeister.Contracts;
using OrleansTicketMeister.Domain.Venue;

namespace OrleansTicketMeister.Silo.Concerts;

public sealed record GetConcertById(Guid Id) : IRequest<GetConcertById.ConcertWithVenueDto>
{
    internal class Handler : IRequestHandler<GetConcertById, ConcertWithVenueDto>
    {
        private readonly TicketMeisterDbContext _dbContext;
        public Handler(TicketMeisterDbContext dbContext)
        {
            _dbContext = dbContext;

        }
        public async Task<ConcertWithVenueDto> Handle(GetConcertById request, CancellationToken cancellationToken)
        {
            Concert? concert = await 
                _dbContext.Concerts
                          .Include(x => x.Venue)
                          .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);
            if (concert is null)
            {
                throw new ArgumentException($"No concert found for id {request.Id}");
            }
            return new ConcertWithVenueDto
            {
                ConcertName = concert.Description,
                VenueName = concert.Venue.Name,
                VenueId = concert.Venue.Id
            };
        }
    }

    public sealed record ConcertWithVenueDto
    {
        public string ConcertName { get; init; } = default!;
        public string VenueName { get; init; } = default!;
        public Guid VenueId { get; init; } = default!;
    }
}
