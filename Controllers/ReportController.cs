using Microsoft.AspNetCore.Mvc;

namespace pos.Controllers
{
	public class ReportController : Controller
	{
		public IActionResult Index()
		{
			
			return View();
		}
	}
}
