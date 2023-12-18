using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using pos.Entities;
using pos.Utils;
using pos.Models.Authencation;
using pos.Services;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace pos.Controllers
{
	public class AuthController : Controller
	{
		private readonly SignInManager<ApplicationUser> _signInManager;
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;
		private IConfiguration _configuration;

		public AuthController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration config)
		{
			_signInManager = signInManager;
			_userManager = userManager;
			_roleManager = roleManager;
			_configuration = config;
		}

		[HttpGet]
		public IActionResult Index()
		{
			ViewData["Title"] = "POS | Login";
			ViewBag.Message = TempData["Message"];
			return View("Login");
		}

		[HttpPost]
		public async Task<IActionResult> Index(LoginModel login)
		{
			if (ModelState.IsValid)
			{
				var user = await _userManager.FindByNameAsync(login.Username);
				if (user != null && await _userManager.CheckPasswordAsync(user, login.Password.Trim()))
				{
					var userRoles = await _userManager.GetRolesAsync(user);

					var authClaims = new List<Claim>
					{
						new Claim(ClaimTypes.Name, login.Username),
						new Claim(ClaimTypes.Email, user.Email),
						new Claim("Avatar", user.Avatar),
						new Claim("FirstLogin", user.FirstLogin.ToString()),
						new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
					};

					foreach (var userRole in userRoles)
					{
						authClaims.Add(new Claim(ClaimTypes.Role, userRole));
					}

					var identityClaims = new ClaimsIdentity(authClaims, "Identity");
					var principal = new ClaimsPrincipal(identityClaims);

					await HttpContext.SignInAsync(principal);

					// Đăng nhập thành công
					return RedirectToAction("Index", "Home");
				}

				TempData["Message"] = "Please check your account again!";
				return RedirectToAction("Index");
			}
			return RedirectToAction("Index", "Home");
		}

		[HttpGet]
		public async Task<IActionResult> Logout()
		{
			await HttpContext.SignOutAsync();
			return RedirectToAction("Index");
		}

		[HttpGet]
		[Route("Auth/Resend/{id}")]
		public async Task<IActionResult> Resend(string id)
		{
			var user = await _userManager.FindByIdAsync(id);

			if (user == null)
			{
				return NotFound();
			}

			var emailConfirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);

			// Thêm timestamp vào URL
			var timestamp = DateTimeOffset.UtcNow.ToString("yyyyMMddHHmmss");
			var confirmationLink = Url.Action("Verify", "Auth", new
			{
				userId = user.Id,
				token = emailConfirmationToken,
				timestamp
			}, Request.Scheme);

			var emailSubject = "Welcome to YourApp";
			var emailContent = $"Thank you for registering with YourApp. Your account has been created successfully." +
							   $" Please confirm your email by clicking <a href='{confirmationLink}'>here</a>.";

			try
			{
				await Helpers.SendEmail(_configuration, user.Email, emailSubject, emailContent);
				return Ok();
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Failed to send email: {ex.Message}");
				return StatusCode(500, "Failed to send email.");
			}
		}

		[HttpGet]
		public async Task<IActionResult> Verify(string userId, string token, string timestamp)
		{
			var user = await _userManager.FindByIdAsync(userId);

			if (user == null)
			{
				TempData["Message"] = "User not found!";
				return RedirectToAction("Error", "Error");
			}

			//// Validate the timestamp
			//if (!Helpers.IsValidTimestamp(timestamp))
			//{
			//	TempData["Message"] = "Link has expired!";
			//	return RedirectToAction("Error", "Error");
			//}

			var result = await _userManager.ConfirmEmailAsync(user, token);

			if (result.Succeeded)
			{
				return RedirectToAction("Index");
			}

			TempData["Message"] = "Verify failed. Please contact admin to help!";
			return RedirectToAction("Error", "Error");
		}

		[HttpGet("/Auth/forgot-password")]
		[AllowAnonymous]
		public IActionResult ForgotPassword([FromForm] string Email)
		{
			ViewData["Title"] = "POS | Forgot-password";
			return View();
		}

		// First Login
		[HttpGet("/Auth/first-login")]
		public IActionResult ChangePasswordFirstLogin()
		{
			ViewBag.Message = TempData["Message"];
			return View("FirstLogin");
		}

		[HttpPost("/Auth/first-login")]
		public async Task<IActionResult> ChangePasswordFirstLogin([FromForm] string password, [FromForm] string confirmPassword)
		{
			var username = HttpContext.User.Identity.Name;

			if (username == null)
			{
				TempData["Message"] = "Cannot found user!";
				return RedirectToAction("ChangePasswordFirstLogin");
			}

			var user = await _userManager.FindByNameAsync(username);
			if (user == null)
			{
				TempData["Message"] = "An Error Occured";
				return RedirectToAction("ChangePasswordFirstLogin");
			}

			var result = await _userManager.ChangePasswordAsync(user, currentPassword: null, newPassword: password);

			if (result.Succeeded)
			{
				await _userManager.ReplaceClaimAsync(user, new Claim("FirstLogin", "True"), new Claim("FirstLogin", "False"));
				return RedirectToAction("/");
			}
			else
			{
				TempData["Message"] = result.Errors.FirstOrDefault()?.Description;
				return RedirectToAction("ChangePasswordFirstLogin");
			}
		}
	}
}
