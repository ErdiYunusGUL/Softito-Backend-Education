using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using FlightSearch.Web.Models;
using FlightSearch.Web.Services;

namespace FlightSearch.Web.Controllers;

public class HomeController : Controller
{
    private readonly ApiService _apiService;

    public HomeController(ApiService apiService)
    {
        _apiService = apiService;
    }

    public async Task<IActionResult> Index(string departureCity, string arrivalCity, DateTime? departureDate, string ticketClass = "economy")
    {
        bool isSearchExecuted = !string.IsNullOrWhiteSpace(departureCity) || !string.IsNullOrWhiteSpace(arrivalCity) || departureDate.HasValue;
        
        IEnumerable<FlightDto> flights = new List<FlightDto>();
        
        if (isSearchExecuted)
        {
            flights = await _apiService.GetFlightsAsync(departureCity, arrivalCity, departureDate, ticketClass);
        }

        ViewBag.DepartureCity = departureCity;
        ViewBag.ArrivalCity = arrivalCity;
        ViewBag.DepartureDate = departureDate?.ToString("yyyy-MM-dd");
        ViewBag.IsSearchExecuted = isSearchExecuted;

        return View(flights);
    }

    public async Task<IActionResult> SearchAirports(string q)
    {
        var airports = await _apiService.SearchAirportsAsync(q);
        return Json(airports);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
