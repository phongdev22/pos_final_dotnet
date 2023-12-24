using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using pos.Entities;

namespace pos.Realtime
{
	public class ProductHub : Hub
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly IHubContext<ProductHub> _hubContext;

		public ProductHub(IHubContext<ProductHub> hubContext, UserManager<ApplicationUser> userManager)
		{
			_hubContext = hubContext;
			_userManager = userManager;
		}

		public void OnAddProduct(Product product)
		{
			Clients.All.SendAsync("AddProduct", product);
		}
	}
}
