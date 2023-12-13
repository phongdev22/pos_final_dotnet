﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pos.Config;
using pos.Entities;

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

		// GET: RetailStores/Create
		public IActionResult Create()
		{
			return View();
		}

		// POST: RetailStores/Create
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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

		// POST: RetailStores/Edit/5
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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

		// GET: RetailStores/Delete/5
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

		// POST: RetailStores/Delete/5
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