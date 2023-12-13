using Microsoft.AspNetCore.Mvc;

namespace pos.Controllers
{
	public class AccountController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
