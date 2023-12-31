﻿using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace pos.Entities
{
	public class Customer
	{
		public int Id { get; set; }

		[Required]
		public string Name { get; set; }
		[Required]
		public string PhoneNumber { get; set; }
		[Required]
		public string Address { get; set; }

		[JsonIgnore]
		public virtual ICollection<Order>? Orders { get; set; }
	}
}
