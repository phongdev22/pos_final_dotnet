using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pos.Config;
using pos.Entities;

namespace pos.Controllers
{
	public class OrdersController : Controller
	{
		private readonly ApplicationDbContext _context;

		public OrdersController(ApplicationDbContext context)
		{
			_context = context;
		}
		
		public async Task<IActionResult> CreateOrder([FromBody] string cartUpdateMode)
		{
			await _context.SaveChangesAsync();
			return View(cartUpdateMode);
		}
	}
}
