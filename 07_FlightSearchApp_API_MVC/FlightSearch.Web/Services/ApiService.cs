using FlightSearch.Web.Models;
using System.Text.Json;

namespace FlightSearch.Web.Services;

public class ApiService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public ApiService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
        var baseUrl = _configuration.GetValue<string>("ApiBaseUrl");
        if (!string.IsNullOrEmpty(baseUrl))
        {
            _httpClient.BaseAddress = new Uri(baseUrl);
        }
    }

    public async Task<IEnumerable<FlightDto>> GetFlightsAsync(string departureCity, string arrivalCity, DateTime? departureDate, string ticketClass = "economy")
    {
        var url = $"/api/flights?departureCity={Uri.EscapeDataString(departureCity ?? "")}&arrivalCity={Uri.EscapeDataString(arrivalCity ?? "")}&ticketClass={Uri.EscapeDataString(ticketClass ?? "")}";
        if (departureDate.HasValue)
        {
            url += $"&departureDate={departureDate.Value.ToString("yyyy-MM-dd")}";
        }

        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        return JsonSerializer.Deserialize<List<FlightDto>>(json, options);
    }

    public async Task<List<AirportDto>?> SearchAirportsAsync(string query)
    {
        var url = $"/api/airports/search?q={Uri.EscapeDataString(query)}";
        var response = await _httpClient.GetAsync(url);
        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<AirportDto>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
        return new List<AirportDto>();
    }

    public async Task<string?> LoginAsync(string email, string password)
    {
        var response = await _httpClient.PostAsJsonAsync("/api/auth/login", new { email, password });
        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<LoginResult>();
            return result?.Token;
        }
        return null;
    }

    public async Task<bool> RegisterAsync(string email, string password)
    {
        var response = await _httpClient.PostAsJsonAsync("/api/auth/register", new { email, password });
        return response.IsSuccessStatusCode;
    }

    public async Task<BookingResult?> CreateBookingAsync(CreateBookingDto request, string token)
    {
        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        var response = await _httpClient.PostAsJsonAsync("/api/bookings", request);
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<BookingResult>();
        }
        return null;
    }
}

public class LoginResult
{
    public string? Token { get; set; }
}

public class BookingResult
{
    public string? Message { get; set; }
    public int BookingId { get; set; }
    public string? PNR { get; set; }
}
