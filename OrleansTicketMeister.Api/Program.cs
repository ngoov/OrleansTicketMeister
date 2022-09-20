using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using OrleansTicketMeister.Silo;
using OrleansTicketMeister.Api;
using OrleansTicketMeister.Contracts;
using OrleansTicketMeister.Silo.Concerts;
using OrleansTicketMeister.Silo.Venues;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
IServiceCollection services = builder.Services;

services.AddDbContext<TicketMeisterDbContext>(options =>
{
    options.UseSqlite("Data Source=C:\\temp\\ticket-meister.db");
});

if (args.Length > 0 && args[0] == "/seed")
{
    services.AddHostedService<SeedWorker>();
}

IClientBuilder? clientBuilder = new ClientBuilder().Configure<ClusterOptions>(options =>
{
    options.ClusterId = "dev";
    options.ServiceId = "ConcertMeister";
});
if (builder.Environment.IsDevelopment())
{
    clientBuilder.UseLocalhostClustering();
}
else
{
    clientBuilder.UseConsulClustering(options =>
    {
        // Run kubectl port-forward service/consul-server --namespace consul 8500:8500
        options.Address = new Uri("https://consul.appsum.com");
    });
}
await using IClusterClient? client = clientBuilder.Build();

await client.Connect();
services.AddMediatR(typeof(GetAllVenues));
services.AddSingleton(client);
WebApplication app = builder.Build();

app.MapGet("/venues", async (IMediator mediator)
    => await mediator.Send(new GetAllVenues()));

app.MapGet("/venues/{id:guid}", async (Guid id, IMediator mediator)
    => await mediator.Send(new GetVenueById(id)));

app.MapGet("/venues/{id:guid}/concerts", async (Guid id, IClusterClient clusterClient)
    => await clusterClient.GetGrain<IVenueGrain>(id).GetConcertsAsync());

app.MapGet("/concerts/{concertId:guid}/seats", async (Guid concertId, IClusterClient clusterClient, IMediator mediator)
    =>
{
    IReadOnlyCollection<SeatsReservationDto> seatsReservations = await clusterClient.GetGrain<IConcertGrain>(concertId).GetSeatsAsync();
    GetConcertById.ConcertWithVenueDto concertWithVenue = await mediator.Send(new GetConcertById(concertId));
    return new ConcertSeatsDto
    {
        ConcertName = concertWithVenue.ConcertName,
        VenueName = concertWithVenue.VenueName,
        SeatsReservations = seatsReservations
    };
});


app.Run();
