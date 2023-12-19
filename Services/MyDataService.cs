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
		public MyDataService(IHttpContextAccessor httpContextAccessor)
		{
			_context = httpContextAccessor;
			GetData();
		}

		public void GetData()
		{
			Email = _context.HttpContext.User.FindFirst(ClaimTypes.Email)?.Value ?? "";

			Name = _context.HttpContext.User.Identity.Name ?? "";

			Avatar = _context.HttpContext.User.FindFirst(p => p.Type.Equals("Avatar"))?.Value ?? "";
		}
	}
}
