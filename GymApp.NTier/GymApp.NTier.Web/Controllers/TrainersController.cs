using GymApp.NTier.DataAccess.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace GymApp.NTier.Web.Controllers
{
    public class TrainersController : Controller
    {
        private readonly GymDbContext _context;

        public TrainersController(GymDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var trainers = await _context.Trainers.ToListAsync();
            return View(trainers);
        }
    }
}
