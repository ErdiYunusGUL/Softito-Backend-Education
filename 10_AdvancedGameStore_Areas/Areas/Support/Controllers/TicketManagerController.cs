using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AdvancedGameStore.Data;
using System.Threading.Tasks;
using System.Linq;

namespace AdvancedGameStore.Areas.Support.Controllers
{
    [Area("Support")]
    public class TicketManagerController : Controller
    {
        private readonly AppDbContext _context;

        public TicketManagerController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var unresolvedTickets = await _context.SupportTickets
                .Include(t => t.Customer)
                .Where(t => !t.IsResolved)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
            return View(unresolvedTickets);
        }
    }
}
