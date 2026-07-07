using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CafePOS.Data;
using CafePOS.Models;

namespace CafePOS.Pages;

public class TableDetailModel : PageModel
{
    private readonly AppDbContext _context;

    public TableDetailModel(AppDbContext context)
    {
        _context = context;
    }

    public CafeTable Table { get; set; } = default!;
    public IList<Category> Categories { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var table = await _context.Tables
            .Include(t => t.Orders.Where(o => !o.IsPaid))
            .ThenInclude(o => o.Product)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (table == null)
        {
            return NotFound();
        }

        Table = table;
        Categories = await _context.Categories
            .Include(c => c.Products)
            .ToListAsync();

        return Page();
    }

    public async Task<IActionResult> OnPostAddProductAsync(int tableId, int productId, string? customerName)
    {
        var table = await _context.Tables.FindAsync(tableId);
        if (table == null) return NotFound();

        if (!table.IsOccupied && !string.IsNullOrWhiteSpace(customerName))
        {
            table.CustomerName = customerName;
        }

        var existingOrder = await _context.Orders
            .FirstOrDefaultAsync(o => o.TableId == tableId && o.ProductId == productId && !o.IsPaid);

        if (existingOrder != null)
        {
            existingOrder.Quantity++;
            existingOrder.CustomerName = table.CustomerName;
        }
        else
        {
            _context.Orders.Add(new Order
            {
                TableId = tableId,
                ProductId = productId,
                Quantity = 1,
                CreatedDate = DateTime.Now,
                IsPaid = false,
                CustomerName = table.CustomerName
            });
        }

        table.IsOccupied = true;
        await _context.SaveChangesAsync();

        return RedirectToPage(new { id = tableId });
    }

    public async Task<IActionResult> OnPostPayAsync(int tableId)
    {
        var table = await _context.Tables
            .Include(t => t.Orders.Where(o => !o.IsPaid))
            .FirstOrDefaultAsync(t => t.Id == tableId);
            
        if (table == null) return NotFound();

        foreach (var order in table.Orders)
        {
            order.IsPaid = true;
            order.PaidDate = DateTime.Now;
        }

        table.IsOccupied = false;
        table.CustomerName = null;
        
        await _context.SaveChangesAsync();

        return RedirectToPage("/Index");
    }
}
