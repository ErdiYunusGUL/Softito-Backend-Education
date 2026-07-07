using GymApp.NTier.DataAccess.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace GymApp.NTier.Web.Controllers
{
    public class PlansController : Controller
    {
        private readonly GymDbContext _context;

        public PlansController(GymDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var plans = await _context.Plans.ToListAsync();
            return View(plans);
        }
    }
}
