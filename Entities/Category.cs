using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace pos.Entities
{
	public class Category
	{
		public int Id { get; set; }

		[Required]
		public string Name { get; set; }

		[JsonIgnore]
		public virtual ICollection<Product>? Products { get; set; }
	}
}
