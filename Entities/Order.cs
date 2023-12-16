using System.ComponentModel.DataAnnotations;

namespace pos.Entities
{
	public class Order
	{
		public int Id { get; set; }
		[Required]
		public string OrderId { get; set; }
		public decimal Total { get; set; }
		//public bool Status { get; set; }

		// public decimal? GivenMoney { get; set; }

		// public DateTime DateCreation { get; set; }
		public virtual Customer Customer { get; set; }
		public virtual ICollection<OrderDetail> OrderDetails { get; set; }
		public virtual ApplicationUser? User { get; set; }
		public virtual RetailStore? RetailStore { get; set; }
		
	}
}
