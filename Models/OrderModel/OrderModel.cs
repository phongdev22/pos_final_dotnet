using pos.Entities;
using pos.Models.OrderModelModel;

namespace pos.Models.Order
{
	public class OrderModel
	{
		//public ApplicationUser User { get; set; }
		public Customer Customer { get; set; }
		public List<OrderDetailModel> Products { get; set; }
		public decimal Total {  get; set; }	
	}
}
