using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using pos.Config;
using pos.Entities;
using pos.Models;
using pos.Models.Product;
using System.Diagnostics;
using System.Reflection.Metadata.Ecma335;

namespace pos.Controllers
{

	public class HomeController : Controller
	{
		private readonly ApplicationDbContext _context;
		public HomeController(ApplicationDbContext context)
		{
			_context = context;
		}

		[Authorize]
		public  IActionResult Index([FromQuery]string collection)
		{
			if (User.IsInRole("Admin"))
			{
				return RedirectToAction("Index", "RetailStores");
			}

			var categories = _context.Categories.ToList();
			ViewBag.Categories = categories;

			if (collection != null)
			{
				var productsCollection = _context.Products.Where(p => p.Category.Name.Equals(collection)).ToList();
				return View(productsCollection);
			}
			var products = _context.Products.ToList();
			return View(products);
		}
	}
}
