using AutoService_Mvc_Identity_Ajax.Data;
using AutoService_Mvc_Identity_Ajax.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AutoService_Mvc_Identity_Ajax.Controllers
{
    [Authorize]
    public class ServiceRecordsController : Controller
    {
        private readonly AppDbContext _context;

        public ServiceRecordsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: ServiceRecords
        public async Task<IActionResult> Index()
        {
            var records = await _context.ServiceRecords
                .Include(s => s.CarModel)
                .ThenInclude(m => m!.Brand)
                .OrderByDescending(s => s.EntryDate)
                .ToListAsync();
            return View(records);
        }

        // GET: ServiceRecords/Create
        public IActionResult Create()
        {
            ViewData["BrandId"] = new SelectList(_context.Brands, "Id", "Name");
            return View();
        }

        // POST: ServiceRecords/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CarModelId,LicensePlate,FaultDescription")] ServiceRecord serviceRecord)
        {
            if (ModelState.IsValid)
            {
                serviceRecord.EntryDate = DateTime.Now;
                _context.Add(serviceRecord);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            
            // Re-populate brands if validation fails
            ViewData["BrandId"] = new SelectList(_context.Brands, "Id", "Name");
            return View(serviceRecord);
        }

        // GET: ServiceRecords/GetModelsByBrand/5
        [HttpGet]
        public async Task<IActionResult> GetModelsByBrand(int brandId)
        {
            var models = await _context.CarModels
                .Where(m => m.BrandId == brandId)
                .Select(m => new { m.Id, m.Name })
                .ToListAsync();
                
            return Json(models);
        }

        // GET: ServiceRecords/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var serviceRecord = await _context.ServiceRecords
                .Include(s => s.CarModel)
                .ThenInclude(m => m!.Brand)
                .Include(s => s.ServiceActions)
                .Include(s => s.ServiceParts)
                .ThenInclude(sp => sp.Part)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (serviceRecord == null) return NotFound();

            return View(serviceRecord);
        }

        // POST: ServiceRecords/AddAction
        [HttpPost]
        public async Task<IActionResult> AddAction([FromBody] ServiceAction serviceAction)
        {
            if (ModelState.IsValid)
            {
                _context.ServiceActions.Add(serviceAction);
                await _context.SaveChangesAsync();
                
                return Json(new { 
                    success = true, 
                    data = new {
                        id = serviceAction.Id,
                        actionName = serviceAction.ActionName,
                        price = serviceAction.Price.ToString("N2")
                    } 
                });
            }

            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
            return Json(new { success = false, message = "Geçersiz veri.", errors = errors });
        }

        // POST: ServiceRecords/AddPart
        [HttpPost]
        public async Task<IActionResult> AddPart([FromBody] ServicePart servicePart)
        {
            if (ModelState.IsValid)
            {
                var part = await _context.Parts.FindAsync(servicePart.PartId);
                if(part == null) return Json(new { success = false, message = "Parça bulunamadı." });

                if(part.StockQuantity < servicePart.Quantity)
                {
                    return Json(new { success = false, message = "Yetersiz stok! Mevcut stok: " + part.StockQuantity });
                }

                // Birim fiyatı stoktaki fiyattan al
                servicePart.UnitPrice = part.UnitPrice;

                // Stoktan düş
                part.StockQuantity -= servicePart.Quantity;

                _context.ServiceParts.Add(servicePart);
                await _context.SaveChangesAsync();
                
                return Json(new { 
                    success = true, 
                    data = new {
                        id = servicePart.Id,
                        partName = part.Name,
                        quantity = servicePart.Quantity,
                        unitPrice = servicePart.UnitPrice.ToString("N2"),
                        totalPrice = (servicePart.Quantity * servicePart.UnitPrice).ToString("N2")
                    } 
                });
            }

            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
            return Json(new { success = false, message = "Geçersiz veri.", errors = errors });
        }
    }
}
