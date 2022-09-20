using System.Net;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using OrleansTicketMeister.Silo;
using OrleansTicketMeister.Domain.Venue;
using OrleansTicketMeister.Silo.Concerts;
using OrleansTicketMeister.Silo.Venues;

// Default port for silo-to-silo communication in a cluster
var siloPort = 11111;
// Default port used when connecting to this silo using a client
var gatewayPort = 30000;
// Default port for the orleans dashboard
var dashboardPort = 8080;
if (args.Length == 1)
{
    siloPort = int.Parse(args[0]);
    gatewayPort = siloPort + 10;
    dashboardPort = siloPort + 20;
}
Console.WriteLine($"Silo port: {siloPort}");
Console.WriteLine($"Gateway port: {gatewayPort}");

IHostBuilder? builder =
    new HostBuilder()
        .ConfigureHostConfiguration(configurationBuilder =>
        {
            configurationBuilder.AddEnvironmentVariables("DOTNET_");
        })
        .ConfigureServices(services =>
        {
            services.AddTransient<IVenueRepository, VenueRepository>();
            services.AddTransient<IConcertSeatsService, ConcertSeatsService>();
            services.AddDbContext<TicketMeisterDbContext>(options =>
            {
                options.UseSqlite("Data Source=C:\\temp\\ticket-meister.db");
            });
            services.AddDbContextFactory<TicketMeisterDbContext>();
        })
        .UseOrleans((hostBuilderContext, siloBuilder) =>
        {
            Console.WriteLine($"Running in environment: {hostBuilderContext.HostingEnvironment.EnvironmentName}");
            if (hostBuilderContext.HostingEnvironment.IsDevelopment())
            {
                siloBuilder.UseLocalhostClustering();
            }
            else
            {
                // Deploy to kubernetes, see https://github.com/ReubenBond/hanbaobao-web/blob/main/HanBaoBaoWeb/Program.cs for an example
                // Use kubernetes-local membership table
                siloBuilder.UseConsulClustering(options =>
                {
                    // Run kubectl port-forward service/consul-server --namespace consul 8500:8500
                    // options.Address = new Uri("http://localhost:8500");
                    // Using K8s Ingress
                    options.Address = new Uri("https://consul.appsum.test");
                });
            }
            siloBuilder
                .AddMemoryGrainStorage("definitions")
                .Configure<ClusterOptions>(options =>
                {
                    options.ClusterId = "dev";
                    options.ServiceId = "ConcertMeister";
                })
                .Configure<EndpointOptions>(options =>
                {
                    options.SiloPort = siloPort;
                    options.GatewayPort = gatewayPort;
                    options.AdvertisedIPAddress = IPAddress.Loopback;
                })
                // Scan the current assembly to have Orleans build the Grain classes from the interfaces found using the nuget Microsoft.Orleans.CodeGenerator.MSBuild
                .ConfigureApplicationParts(
                    parts => parts.AddApplicationPart(typeof(ConcertGrain).Assembly).WithReferences())
                .ConfigureLogging(logging => logging.AddConsole())
                .UseDashboard(options =>
                {
                    options.Port = dashboardPort;
                });
        });

IHost? host = builder.Build();
await host.StartAsync();
Console.WriteLine("Orleans is running.\nPress Enter to terminate...");
Console.ReadLine();

Console.WriteLine("Orleans is stopping...");
await host.StopAsync();
