using Microsoft.AspNetCore.Identity;

namespace pos.Entities
{
	public class ApplicationUser : IdentityUser
	{
		public string Gender { get; set; } = "Male";
		public bool Active { get; set; } = false;
		public bool FirstLogin { get; set; } = true;
		public string Avatar { get; set; } = "~/images/default/profile/user-1.png";
	}
}
