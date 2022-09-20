using MediatR;
using OrleansTicketMeister.Contracts;
using OrleansTicketMeister.Domain.Venue;

namespace OrleansTicketMeister.Silo.Venues;

public sealed record GetVenueById(Guid Id) : IRequest<VenueDto>
{
    internal class Handler : IRequestHandler<GetVenueById, VenueDto>
    {
        private readonly TicketMeisterDbContext _dbContext;
        public Handler(TicketMeisterDbContext dbContext)
        {
            _dbContext = dbContext;

        }
        public async Task<VenueDto> Handle(GetVenueById request, CancellationToken cancellationToken)
        {
            Venue? venue = await _dbContext.Venues.FindAsync(new object[] { request.Id }, cancellationToken);
            if (venue is null)
            {
                throw new ArgumentException($"No venue found for id {request.Id}");
            }
            return new VenueDto
            {
                Id = venue.Id,
                Name = venue.Name
            };
        }
    }
}
