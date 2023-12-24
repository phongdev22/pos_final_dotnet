using Microsoft.AspNetCore.SignalR;
using pos.Entities;

namespace pos.Realtime
{
	public class ProductHub : Hub
	{
		private readonly IHubContext<ProductHub> _hubContext;

		public ProductHub(IHubContext<ProductHub> hubContext)
		{
			_hubContext = hubContext;
		}

		public void OnAddProduct(Product product)
		{

		}
	}
}
