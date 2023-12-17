using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pos.Config;
using pos.Entities;
using pos.Models.Order;

namespace pos.Controllers
{
	public class OrdersController : Controller
	{
		private readonly ApplicationDbContext _context;

		public OrdersController(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<IActionResult> Index()
		{
			var orders = await _context.Orders.ToListAsync();
			return View(orders);
		}

		[HttpPost]
		public async Task<IActionResult> Create([FromBody] OrderModel orders)
		{

			var cusPhoneNumber = orders.Customer.PhoneNumber;
			var customer = _context.Customer.FirstOrDefault(cus => cus.PhoneNumber.Equals(cusPhoneNumber));

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

			var order = new Order()
			{
				OrderId = Guid.NewGuid().ToString(),
				Total = orders.Total,
				Customer = customer,
			};

			foreach (var detail in orders.Products)
			{
				var pId = Convert.ToInt32(detail.Id);
				var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == pId);

				var od = new OrderDetail()
				{
					Subtotal = detail.Subtotal,
					Quantity = detail.Quantity,
					Order = order,
					Product = product
				};
				_context.OrderDetails.Add(od);
				order.OrderDetails.Add(od);
			}

			_context.Orders.Add(order);

			var result = await _context.SaveChangesAsync();

			if (result > 0)
			{
				return Ok(new { code = 0, returnUrl = "/Orders/Checkout/" + order.OrderId });
			}
			else
			{
				return Ok(new
				{
					code = 1,
					message = "Create order fail!"
				});
			}
		}
		
		[HttpGet("/Orders/Checkout/{id}")]
		public async Task<IActionResult> Checkout(string? id)
		{
			var order = await _context.Orders.FirstOrDefaultAsync(od => od.OrderId.Equals(id));

			return View(order);
		}
		
	}
}
