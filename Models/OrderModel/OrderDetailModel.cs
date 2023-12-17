namespace pos.Models.OrderModelModel
{
	public class OrderDetailModel
	{
		public string Id { get; set; }
		public string Name { get; set; }
		public decimal Price { get; set; }
		public int Quantity { get; set; }
		public decimal Subtotal { get; set; }
		public int Max {  get; set; }
	}
}
