namespace pos.Models.Product
{
	public class ProductSearchResponse
	{
		public string Id { get; set; }
		public string Name { get; set; }
		public string Barcode { get; set; }
		public decimal Price { get; set; }
		public int Quantity { get; set; }
		public string ImagePath { get; set; }
	}
}
