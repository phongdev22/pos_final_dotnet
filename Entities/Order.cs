using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace pos.Entities
{
	public class Order
	{
		public int Id { get; set; }
		[Required]
		public string OrderId { get; set; }
		public decimal Total { get; set; } = 0;
		public bool Status { get; set; } = false;
		public decimal GivenMoney { get; set; } = 0;
		public DateTime DateCreation { get; set; } = DateTime.Now;
		public virtual Customer Customer { get; set; }
		[JsonIgnore]
		public virtual ICollection<OrderDetail> OrderDetails { get; set; }
		public virtual ApplicationUser? User { get; set; }
		public virtual RetailStore? RetailStore { get; set; }
	}
}
