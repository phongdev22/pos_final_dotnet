﻿using System.Text.Json.Serialization;

namespace pos.Entities
{
	public class RetailStore
	{
		public int Id { get; set; }
		public required string StoreName { get; set; }
		public required string Location { get; set; }
		[JsonIgnore]
		public virtual ICollection<Inventory>? Inventories { get; set; }

		[JsonIgnore]
		public virtual ICollection<ApplicationUser>? AppUsers { get; set; }
	}
}
