using System.ComponentModel.DataAnnotations;

namespace pos.Models.Authencation
{
	public class LoginView
	{
		[Required(ErrorMessage = "User Name is required")]
		public string Username { get; set; }

		[Required(ErrorMessage = "Password is required")]
		public string Password { get; set; }
		public bool Remember { get; set; }
	}
}
