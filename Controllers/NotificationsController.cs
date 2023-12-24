using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace pos.Controllers
{
    public class NotificationsController : Controller
    {
        [AllowAnonymous]
        [Route("Error/{statusCode}")]
        public IActionResult HttpStatusCodeHandler(int statusCode)
        {
            var statusCodeResult = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();

            ViewBag.ErrorMessage = TempData["Message"];

			switch (statusCode)
            {
                case 404:
                    ViewBag.ErrorMessage = "Sorry, the resource you requested could not be found.";
                    break;
                case 505:
                    break;
            }

            return View("Error");
        }

        [Route("Error")]
        [AllowAnonymous]
        public IActionResult Error()
        {
            var exceptionHandlerPathFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
			ViewBag.Message = TempData["Message"];
			return View("Error");
        }

        public IActionResult AccessDenied()
        {
            return View("Error");
        }

		// [AllowAnonymous]
		public IActionResult Failed()
        {
            ViewBag.Message = TempData["Message"] == null ? "Failed !" : TempData["Message"];
            return View();
        }

		// [AllowAnonymous]
		public IActionResult Success()
        {
            ViewBag.Message = TempData["Message"] == null ? "Success !" : TempData["Message"];
            return View();
        }

	}
}
