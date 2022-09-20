using OrleansTicketMeister.Domain.Venue;
using OrleansTicketMeister.Silo;

namespace OrleansTicketMeister.Api;

public class SeedWorker : IHostedService
{

    private readonly IServiceProvider _serviceProvider;

    public SeedWorker(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using IServiceScope scope = _serviceProvider.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<TicketMeisterDbContext>();
        await context.Database.EnsureDeletedAsync(cancellationToken);
        await context.Database.EnsureCreatedAsync(cancellationToken);
        foreach (Venue venue in GetVenues())
        {
            context.Venues.Add(venue);
        }
        await context.SaveChangesAsync(cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    private static IEnumerable<Venue> GetVenues()
    {
        var abBrussels = new Venue(new Guid("d11eca17-1e67-492e-bb5c-64a7e07e57ad"), "AB Brussels");
        abBrussels.RegisterConcert(new Concert(new Guid("812e0223-dc18-4260-a769-56ebf2241100"), "HammerFall", new DateTime(2022, 10, 23)));
        abBrussels.RegisterConcert(new Concert(new Guid("8f778a81-a2b3-49e8-820b-147cb4f63c29"), "Epica", new DateTime(2023, 1, 12)));
        abBrussels.RegisterConcert(new Concert(new Guid("00d19df7-c37c-4e57-ad9b-db79d2544ff9"), "Killswitch Engage", new DateTime(2023, 3, 6)));
            
        var sportpaleisAntwerp = new Venue(new Guid("618f49ae-74e2-4fd9-8266-949c93afad1f"), "Sportpaleis Antwerp");
        sportpaleisAntwerp.RegisterConcert(new Concert(new Guid("f567217c-adbf-421e-a061-8570285faf8e"), "Sabaton", new DateTime(2022, 11, 14)));
            
        var lottoArenaAntwerp = new Venue(new Guid("9cd6a02b-1ade-463e-880e-14b7469d7fe9"),  "Lotto Arena Antwerp");
        lottoArenaAntwerp.RegisterConcert(new Concert(new Guid("e7adbf9e-6b6d-45cb-b99c-f2f5757bb5e4"), "Powerwolf", new DateTime(2022, 12, 13)));
        lottoArenaAntwerp.RegisterConcert(new Concert(new Guid("adcea554-4e4d-4e7b-926c-f0b0ca4613d7"), "Trivium", new DateTime(2023, 5, 25)));
            
        var boudewijnStationBrussels = new Venue(new Guid("a835ba54-9d0f-4127-b41f-23e629af7079"), "Boudewijn Stadion Brussels");
        boudewijnStationBrussels.RegisterConcert(new Concert(new Guid("149a9712-b895-443f-a58e-6f7932799019"), "Rammstein", new DateTime(2023, 7, 11)));
            
        return new[]
        {
            abBrussels,
            sportpaleisAntwerp,
            lottoArenaAntwerp,
            boudewijnStationBrussels,
        };
    }
}
