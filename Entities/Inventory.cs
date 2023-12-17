using System.ComponentModel.DataAnnotations.Schema;

namespace pos.Entities
{
	public class Inventory
	{
		public int Id { get; set; }
		public int Quantity { get; set; }

		[ForeignKey("RetailStore")]
		public int RetailStoreId { get; set; }
		public virtual RetailStore RetailStore { get; set; }

		[ForeignKey("Product")]
		public int ProductId { get; set; }
		public virtual Product? Product { get; set; }
	}
}
