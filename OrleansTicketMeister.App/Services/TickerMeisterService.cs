using System.Text.Json;
using OrleansTicketMeister.Contracts;

namespace OrleansTicketMeister.App.Services;

public class TickerMeisterService
{
    private readonly HttpClient _httpClient;
    public TickerMeisterService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ICollection<VenueDto>?> GetVenuesAsync(CancellationToken cancellationToken = default)
        => await GetDataFromApiAsync<ICollection<VenueDto>>($"venues", cancellationToken);
    public async Task<VenueConcertsDto?> GetConcertsByVenueIdAsync(Guid venueId, CancellationToken cancellationToken = default) 
        => await GetDataFromApiAsync<VenueConcertsDto>($"venues/{venueId}/concerts", cancellationToken);
    public async Task<VenueDto?> GetVenueByIdAsync(Guid venueId, CancellationToken cancellationToken = default)
        => await GetDataFromApiAsync<VenueDto>($"venues/{venueId}", cancellationToken);
    public async Task<ConcertSeatsDto?> GetConcertSeatsAsync(Guid? concertId, CancellationToken cancellationToken = default)
        => await GetDataFromApiAsync<ConcertSeatsDto>($"concerts/{concertId}/seats", cancellationToken);
    
    private async Task<T?> GetDataFromApiAsync<T>(string url, CancellationToken cancellationToken = default)
    {
        HttpResponseMessage httpResponseMessage = await _httpClient.GetAsync(url, cancellationToken);
        httpResponseMessage.EnsureSuccessStatusCode();
        string json = await httpResponseMessage.Content.ReadAsStringAsync(cancellationToken);
        return JsonSerializer.Deserialize<T?>(json, new JsonSerializerOptions{
            PropertyNameCaseInsensitive = true
        });
    }

}
