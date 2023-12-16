using pos.Entities;

namespace pos.Models.Order
{
	public class OrderModel
	{
		public ApplicationUser User { get; set; }
		public Customer Custemer { get; set; }
		public List<OrderDetail> OrderDetails { get; set; }
		public RetailStore RetailStore { get; set; }
		public decimal Total {  get; set; }	
	}
}
