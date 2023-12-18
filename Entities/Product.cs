using System.ComponentModel.DataAnnotations.Schema;

namespace pos.Entities
{
	public class Product
	{
		public int Id { get; set; }
		public required string Name { get; set; }
		public required decimal Price { get; set; }
		public required decimal Cost { get; set; }
		public required string Barcode { get; set; }
		public required int Quantity { get; set; }
		public string ImagePath { get; set; } = "/images/default/product/no-image.png";
		public bool isDelete { get; set; } = true;

		[ForeignKey("Category")]
		public int? CategoryId;
		public virtual Category? Category { get; set; }
		public virtual ICollection<Inventory> Inventories { get; set; }
	}
}
