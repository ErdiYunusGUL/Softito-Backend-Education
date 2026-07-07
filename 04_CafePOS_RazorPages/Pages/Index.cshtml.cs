using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CafePOS.Data;
using CafePOS.Models;

namespace CafePOS.Pages;

public class IndexModel : PageModel
{
    private readonly AppDbContext _context;

    public IndexModel(AppDbContext context)
    {
        _context = context;
    }

    public IList<CafeTable> Tables { get; set; } = default!;

    public async Task OnGetAsync()
    {
        Tables = await _context.Tables
            .Include(t => t.Orders.Where(o => !o.IsPaid))
            .ThenInclude(o => o.Product)
            .ToListAsync();
    }
}
