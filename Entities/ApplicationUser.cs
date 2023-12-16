using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace pos.Entities
{
	public class ApplicationUser : IdentityUser
	{

		public string FullName { get; set; } = string.Empty;
		public string Gender { get; set; } = "Male";
		public bool Active { get; set; } = false;
		public bool FirstLogin { get; set; } = true;
		public string Avatar { get; set; } = "/images/default/profile/user-1.png";

		[ForeignKey("RetailStoreId")]
		public int? RetailStoreId { get; set; }
		public virtual RetailStore? RetailStore { get; set; } = null;
	}
}
