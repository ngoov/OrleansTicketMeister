using MediatR;
using Microsoft.EntityFrameworkCore;
using OrleansTicketMeister.Contracts;

namespace OrleansTicketMeister.Silo.Venues;

public sealed record GetAllVenues : IRequest<IReadOnlyCollection<VenueDto>>
{
    internal class Handler : IRequestHandler<GetAllVenues, IReadOnlyCollection<VenueDto>>
    {
        private readonly TicketMeisterDbContext _dbContext;
        public Handler(TicketMeisterDbContext dbContext)
        {
            _dbContext = dbContext;

        }
        public async Task<IReadOnlyCollection<VenueDto>> Handle(GetAllVenues request, CancellationToken cancellationToken) 
            => await _dbContext.Venues.Select(v => new VenueDto
            {
                Id = v.Id,
                Name = v.Name
            }).ToListAsync(cancellationToken: cancellationToken);
    }
}
