using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using pos.Config;
using pos.Models;
using System.Diagnostics;

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
		public async Task<IActionResult> Index()
		{

			var authenticationResult = await HttpContext.AuthenticateAsync();
			var token = authenticationResult?.Properties?.GetTokenValue("access_token");

			return View();
		}
		[Authorize]
		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
