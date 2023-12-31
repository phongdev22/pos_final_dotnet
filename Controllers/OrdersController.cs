﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
			
			var currentUserName = User.Identity.Name;

			var orders = new List<Order>();

			if (User.IsInRole("Admin"))
			{
				orders = await _context.Orders.ToListAsync();
			}
			else
			{
				var currentUser = await _userManager.FindByNameAsync(currentUserName);

				if (currentUser != null)
				{
					orders = await _context.Orders.Where(o => o.User.RetailStoreId == currentUser.RetailStoreId).ToListAsync();
				}
			}

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

			// Detail Order
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

				// update quantity product main
				product.Quantity = product.Quantity - od.Quantity;

				// update product in inventory
				var ivnentory = _context.Inventory.FirstOrDefault(inven => inven.ProductId == pId && inven.Id == detail.Inventory);
				
				if(ivnentory != null)
				{
					ivnentory.Quantity = ivnentory.Quantity - od.Quantity;
				}

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

			foreach (var orderDetail in order.OrderDetails)
			{
				var product = _context.Products.FirstOrDefault(p => p.Id == orderDetail.ProductId);
				if(product != null)
				{
					product.Quantity = product.Quantity -  orderDetail.Quantity;
					product.isDelete = false;
				}
			}

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
