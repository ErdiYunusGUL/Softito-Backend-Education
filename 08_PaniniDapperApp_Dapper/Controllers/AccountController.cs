using Microsoft.AspNetCore.Mvc;
using PaniniDapperApp.Repositories;

namespace PaniniDapperApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly StickerRepository _repository;

        public AccountController(StickerRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            var user = await _repository.AuthenticateAsync(username, password);
            if (user != null)
            {
                HttpContext.Session.SetInt32("ActiveCollectorId", user.Id);
                HttpContext.Session.SetString("ActiveCollectorName", user.Username);
                return RedirectToAction("Index", "Home");
            }
            
            ViewBag.Error = "Invalid username or password. Try 'ali' / '12345'";
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
