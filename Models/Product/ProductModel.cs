using pos.Entities;

namespace pos.Models.Product
{
	public class ProductModel
	{
		public pos.Entities.Product? Product { get; set; }
		public List<Category>? Categories { get; set; }
		public List<RetailStore>? RetailStores { get; set; }
	}
}
