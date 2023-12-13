using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using pos.Config;
using pos.Entities;

namespace pos.Controllers
{
	public class InventoriesController : Controller
	{
		private readonly ApplicationDbContext _context;

		public InventoriesController(ApplicationDbContext context)
		{
			_context = context;
		}

		// GET: Inventories
		public async Task<IActionResult> Index()
		{
			var applicationDbContext = _context.Inventory.Include(i => i.Product).Include(i => i.RetailStore);
			return View(await applicationDbContext.ToListAsync());
		}

		// GET: Inventories/Details/5
		public async Task<IActionResult> Details(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var inventory = await _context.Inventory
				.Include(i => i.Product)
				.Include(i => i.RetailStore)
				.FirstOrDefaultAsync(m => m.Id == id);
			if (inventory == null)
			{
				return NotFound();
			}

			return View(inventory);
		}

		// GET: Inventories/Create
		public IActionResult Create()
		{
			ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Id");
			ViewData["RetailSotreId"] = new SelectList(_context.RetailStores, "Id", "Id");
			return View();
		}

		// POST: Inventories/Create
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("Id,Quantity,RetailSotreId,ProductId")] Inventory inventory)
		{
			if (ModelState.IsValid)
			{
				_context.Add(inventory);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}
			ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Id", inventory.ProductId);
			ViewData["RetailSotreId"] = new SelectList(_context.RetailStores, "Id", "Id", inventory.RetailStoreId);
			return View(inventory);
		}

		// GET: Inventories/Edit/5
		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var inventory = await _context.Inventory.FindAsync(id);
			if (inventory == null)
			{
				return NotFound();
			}
			ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Id", inventory.ProductId);
			ViewData["RetailSotreId"] = new SelectList(_context.RetailStores, "Id", "Id", inventory.RetailStoreId);
			return View(inventory);
		}

		// POST: Inventories/Edit/5
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("Id,Quantity,RetailSotreId,ProductId")] Inventory inventory)
		{
			if (id != inventory.Id)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					_context.Update(inventory);
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!InventoryExists(inventory.Id))
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
			ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Id", inventory.ProductId);
			ViewData["RetailSotreId"] = new SelectList(_context.RetailStores, "Id", "Id", inventory.RetailStoreId);
			return View(inventory);
		}

		// GET: Inventories/Delete/5
		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var inventory = await _context.Inventory
				.Include(i => i.Product)
				.Include(i => i.RetailStore)
				.FirstOrDefaultAsync(m => m.Id == id);
			if (inventory == null)
			{
				return NotFound();
			}

			return View(inventory);
		}

		// POST: Inventories/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var inventory = await _context.Inventory.FindAsync(id);
			if (inventory != null)
			{
				_context.Inventory.Remove(inventory);
			}

			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

		private bool InventoryExists(int id)
		{
			return _context.Inventory.Any(e => e.Id == id);
		}
	}
}
