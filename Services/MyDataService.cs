using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace pos.Services
{
	public class MyDataService
	{
		private IHttpContextAccessor _context;
		public string Username {  get; set; }
		public string Email { get; set; }
		public string Avatar { get; set; }
		public string Name { get; set; }
		public bool IsAdmin { get; set; }
		public bool IsManager { get; set; }
		public bool IsEmployee { get; set; }

		public MyDataService(IHttpContextAccessor httpContextAccessor)	
		{
			_context = httpContextAccessor;
			GetData();
		}

		public void GetData()
		{
            Name = _context.HttpContext.User.FindFirst(ClaimTypes.Name)?.Value ?? "";

            Email = _context.HttpContext.User.FindFirst(ClaimTypes.Email)?.Value ?? "";

			Username = _context.HttpContext.User.Identity.Name ?? "";

			Avatar = _context.HttpContext.User.FindFirst(p => p.Type.Equals("Avatar"))?.Value ?? "";

			IsAdmin = _context.HttpContext.User.IsInRole("Admin");
			IsManager = _context.HttpContext.User.IsInRole("Manager");
			IsEmployee = _context.HttpContext.User.IsInRole("Employee");
		}
	}
}
