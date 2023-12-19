using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pos.Config;
using pos.Entities;

namespace pos.Controllers
{
	[Authorize]
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

			return Ok(customer);
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

		[HttpGet]
		public async Task<IActionResult> Search([FromQuery] string keyword)
		{
			var customer = await _context.Customer.Where(c => c.PhoneNumber.Equals(keyword)).FirstOrDefaultAsync();

			if (customer == null)
			{
				return Ok(new { code = 1, message = "Not Found!" });
			}

			return Ok(new
			{
				code = 0,
				customer = new
				{
					customer.Id,
					customer.Name,
					customer.Address,
				}
			});
		}

		[HttpDelete]
		public async Task<IActionResult> Delete(int id)
		{
            var customer = await _context.Customer.FirstOrDefaultAsync(od => od.Id == id);

            if (customer == null) return Ok(new { code = 1, Message = "Not found!" });

            _context.Customer.Remove(customer);
            _context.SaveChanges();

            return Ok(new { code = 0, Message = "Success" });
        }


		private bool CustomerExists(int id)
		{
			return _context.Customer.Any(e => e.Id == id);
		}
	}
}
