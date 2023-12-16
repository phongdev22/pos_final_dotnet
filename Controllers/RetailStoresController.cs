using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pos.Config;
using pos.Entities;
using pos.Models.Store;
using System.Net.WebSockets;

namespace pos.Controllers
{
    public class RetailStoresController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RetailStoresController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: RetailStores
        public async Task<IActionResult> Index()
        {
            return View(await _context.RetailStores.ToListAsync());
        }

        // GET: RetailStores/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var orders = _context.Orders
                .Where(or => or.RetailStore.Id == id)
                .ToList();

            var totalProductsSold = orders
                .SelectMany(o => o.OrderDetails)
                .Sum(od => od.Quantity);

            var totalRevenuePerStore = orders
                .Sum(o => o.Total);

            var topSellingUser = _context.Orders
                .GroupBy(o => o.User)
                .OrderByDescending(g => g.Count())
                .Select(g => g.Key)
                .FirstOrDefault();

            var topSpendingCustomer = _context.Orders
                .GroupBy(o => o.Customer)
                .OrderByDescending(g => g.Sum(o => o.Total))
                .Select(g => g.Key)
                .FirstOrDefault();

            var DetailModel = new DetailStoreModel()
            {
                Orders = orders,
                TotalProductsSold = totalProductsSold,
                TotalRevenuePerStore = totalRevenuePerStore,
                TopSpendingCustomer = topSpendingCustomer,
                TopSellingUser = topSellingUser
            };

            return View(DetailModel);
        }

        // GET: RetailStores/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,StoreName,Location")] RetailStore retailStore)
        {
            if (ModelState.IsValid)
            {
                _context.Add(retailStore);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(retailStore);
        }

        // GET: RetailStores/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var retailStore = await _context.RetailStores.FindAsync(id);
            if (retailStore == null)
            {
                return NotFound();
            }
            return View(retailStore);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,StoreName,Location")] RetailStore retailStore)
        {
            if (id != retailStore.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(retailStore);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RetailStoreExists(retailStore.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(retailStore);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var retailStore = await _context.RetailStores
                .FirstOrDefaultAsync(m => m.Id == id);
            if (retailStore == null)
            {
                return NotFound();
            }

            return View(retailStore);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var retailStore = await _context.RetailStores.FindAsync(id);
            if (retailStore != null)
            {
                _context.RetailStores.Remove(retailStore);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RetailStoreExists(int id)
        {
            return _context.RetailStores.Any(e => e.Id == id);
        }
    }
}
