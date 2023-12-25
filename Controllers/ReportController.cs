using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pos.Config;
using pos.Entities;
using pos.Models.Store;
using System.Globalization;

namespace pos.Controllers
{
	public class ReportController : Controller
	{
		private readonly ApplicationDbContext _context;

		public ReportController(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<IActionResult> Index([FromQuery] int store, [FromQuery] string start, [FromQuery] string end)
		{
			string timestampFormat = "yyyyMMddHHmmss";

			if (DateTime.TryParseExact(start, timestampFormat, null, DateTimeStyles.None, out DateTime startTime)
				&& DateTime.TryParseExact(end, timestampFormat, null, DateTimeStyles.None, out DateTime endTime))
			{
				var orders = await _context.Orders.Where(order => order.DateCreation >= startTime && order.DateCreation <= endTime && order.User.RetailStoreId == store ).ToListAsync();



				var totalProductsSold = orders
				.SelectMany(o => o.OrderDetails)
				.Sum(od => od.Quantity);

				var totalRevenuePerStore = orders
					.Sum(o => o.Total);

				var topSellingUser = orders
					.GroupBy(o => o.User)
					.OrderByDescending(g => g.Count())
					.Select(g => g.Key)
					.FirstOrDefault();

				var topSpendingCustomer = orders
					.GroupBy(o => o.Customer)
					.OrderByDescending(g => g.Sum(o => o.Total))
					.Select(g => g.Key)
					.FirstOrDefault();

				var DetailModel = new DetailStoreModel()
				{
					// RetailStore = await _context.RetailStores.FindAsync(store),
					Orders = orders,
					TotalProductsSold = totalProductsSold,
					TotalRevenuePerStore = totalRevenuePerStore,
					TopSpendingCustomer = topSpendingCustomer,
					TopSellingUser = topSellingUser
				};

				return Ok(new { data = DetailModel});
			}
			else
			{
				return Ok(new { Code = 1 });
			}

		}
	}

}
