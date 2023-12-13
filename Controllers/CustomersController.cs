﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pos.Config;
using pos.Entities;

namespace pos.Controllers
{
	public class CustomersController : Controller
	{
		private readonly ApplicationDbContext _context;

		public CustomersController(ApplicationDbContext context)
		{
			_context = context;
		}

		// GET: Customers
		public async Task<IActionResult> Index()
		{
			return View(await _context.Customer.ToListAsync());
		}

		// GET: Customers/Details/5
		public async Task<IActionResult> Details(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var customer = await _context.Customer
				.FirstOrDefaultAsync(m => m.Id == id);
			if (customer == null)
			{
				return NotFound();
			}

			return View(customer);
		}

		// GET: Customers/Create
		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("Id,Name,PhoneNumber,Address")] Customer customer)
		{
			if (ModelState.IsValid)
			{
				_context.Add(customer);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}
			return View(customer);
		}

		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var customer = await _context.Customer.FindAsync(id);
			if (customer == null)
			{
				return NotFound();
			}
			return View(customer);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("Id,Name,PhoneNumber,Address")] Customer customer)
		{
			if (id != customer.Id)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					_context.Update(customer);
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!CustomerExists(customer.Id))
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
			return View(customer);
		}

		// GET: Customers/Delete/5
		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var customer = await _context.Customer
				.FirstOrDefaultAsync(m => m.Id == id);
			if (customer == null)
			{
				return NotFound();
			}

			return View(customer);
		}

		// POST: Customers/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var customer = await _context.Customer.FindAsync(id);
			if (customer != null)
			{
				_context.Customer.Remove(customer);
			}

			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

		private bool CustomerExists(int id)
		{
			return _context.Customer.Any(e => e.Id == id);
		}
	}
}