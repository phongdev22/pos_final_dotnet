using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using pos.Config;
using pos.Entities;
using pos.Models.Order;

namespace pos.Controllers
{
	[Authorize]
	public class OrdersController : Controller
	{
		private readonly ApplicationDbContext _context;
		private readonly UserManager<ApplicationUser> _userManager;

		public OrdersController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
		{
			_context = context;
			_userManager = userManager;
		}

		public async Task<IActionResult> Index()
		{
			var orders = await _context.Orders.ToListAsync();
			return View(orders);
		}

		[HttpGet]
		public async Task<IActionResult> History(string id)
		{
			var result = await _context.Orders.Where(or => or.Customer.PhoneNumber.Equals(id)).ToListAsync();
			ViewBag.TotalSpending = result.Sum(or => or.Total);
			ViewBag.TotalOrder = result.Count();
			return View(result);
		}

		[HttpGet]
		public async Task<IActionResult> Details(string? id)
		{
			var order = await _context.Orders.FirstOrDefaultAsync(order => order.OrderId.Equals(id));

			if (order == null) return NotFound();

			if (!order.Status) return RedirectToAction("Checkout", new { id = order.OrderId });

			return View(order);
		}

		[HttpGet]
		public async Task<IActionResult> Search(string phoneNumber, [FromQuery]string start, [FromQuery]string end)
		{
			var orders = await _context.Orders.Where(od => od.Customer.PhoneNumber.Equals(phoneNumber)).ToListAsync();	
			return View(orders);
		}

		[HttpPost]
		public async Task<IActionResult> Create([FromBody] OrderModel orders)
		{
			var billerName = HttpContext.User.Identity.Name;

			var cusPhoneNumber = orders.Customer.PhoneNumber;
			var customer = _context.Customer.FirstOrDefault(cus => cus.PhoneNumber.Equals(cusPhoneNumber));

			// Customer
			if (customer == null)
			{
				var newCustomer = new Customer
				{
					Address = orders.Customer.Address,
					PhoneNumber = cusPhoneNumber,
					Name = orders.Customer.Name,
				};
				_context.Add(newCustomer);
				customer = newCustomer;
			}

			// Create Order
			var order = new Order()
			{
				OrderId = Guid.NewGuid().ToString(),
				Total = orders.Total,
				Customer = customer,
			};

			// Biller
			if (billerName != null)
			{
				var biller = await _userManager.FindByNameAsync(billerName);
				if (biller != null)
				{
					order.User = biller;
				}
			}

		

		[HttpGet]
		public async Task<IActionResult> Checkout(string id)
		{
			var order = await _context.Orders.FirstOrDefaultAsync(od => od.OrderId.Equals(id));
			return View(order);
		}

		[HttpPost]
		public async Task<IActionResult> Complete(string id, [FromBody] Order cash)
		{
			var order = await _context.Orders.FirstOrDefaultAsync(od => od.OrderId.Equals(id));

			if (order == null) return Ok(new { code = 1, Message = "Not found!" }); ;
			order.GivenMoney = cash.GivenMoney;
			order.Status = true;

			await _context.SaveChangesAsync();

			return Ok(new { code = 0, returnUrl = "/", messsage = "Complete success!" });
		}

		[HttpDelete]
		public async Task<IActionResult> Delete(int id)
		{
			var order = await _context.Orders.FirstOrDefaultAsync(od => od.Id == id);

			if (order == null) return Ok(new { code = 1, Message = "Not found!" });

			_context.Orders.Remove(order);
			_context.SaveChanges();

			return Ok(new { code = 0, Message = "Success" });
		}
	}
}
