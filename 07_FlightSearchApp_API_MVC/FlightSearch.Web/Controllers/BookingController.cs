using FlightSearch.Web.Models;
using FlightSearch.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FlightSearch.Web.Controllers;

[Authorize]
public class BookingController : Controller
{
    private readonly ApiService _apiService;

    public BookingController(ApiService apiService)
    {
        _apiService = apiService;
    }

    [HttpGet]
    public IActionResult Checkout(int flightId)
    {
        var model = new CreateBookingDto { FlightId = flightId };
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Checkout(CreateBookingDto model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var token = User.FindFirst("jwt_token")?.Value;
        if (string.IsNullOrEmpty(token))
        {
            return RedirectToAction("Login", "Auth");
        }

        var result = await _apiService.CreateBookingAsync(model, token);
        if (result != null)
        {
            return RedirectToAction("Success", new { pnr = result.PNR });
        }

        ModelState.AddModelError("", "Booking failed. Please try again.");
        return View(model);
    }

    public IActionResult Success(string pnr)
    {
        return View("Success", pnr);
    }
}
