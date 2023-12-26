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
using System.Security.Principal;

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
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    };

                    // Check first Login
                    if (user.FirstLogin)
                    {
                        authClaims.Add(new Claim("FirstLogin", user.FirstLogin.ToString()));
                    }

                    foreach (var userRole in userRoles)
                    {
                        authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                    }

                    // Sign Indentity
                    var identityClaims = new ClaimsIdentity(authClaims, "Identity");
                    var principal = new ClaimsPrincipal(identityClaims);

                    await HttpContext.SignInAsync(principal);

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
                return RedirectToAction("Error", "Notifications");
            }

            var result = await _userManager.ConfirmEmailAsync(user, token);

            if (result.Succeeded)
            {
                TempData["Message"] = "Verify Success.";
                return RedirectToAction("Success", "Notifications");
            }

            TempData["Message"] = "Verify failed. Please contact admin to help!";
            return RedirectToAction("Failed", "Notifications");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            ViewData["Title"] = "POS | Forgot-password";
            return View();
        }

		[HttpPost]
		[AllowAnonymous]
		public async Task<IActionResult> ForgotPassword([FromForm] string Email)
		{
			// Check if the email is provided
			if (string.IsNullOrEmpty(Email))
			{
				// Handle the case where email is not provided
				ModelState.AddModelError("Email", "Email is required.");
				return View();
			}

			// Find the user by email
			var user = await _userManager.FindByEmailAsync(Email);
			if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
			{
				return RedirectToAction("ForgotPasswordConfirmation");
			}

			// TODO: Send the reset link to the user via email
			return RedirectToAction("ForgotPasswordConfirmation");
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

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, password);

            if (result.Succeeded)
            {
                // First Login
                user.FirstLogin = false;
                await _userManager.UpdateAsync(user);

                // Remove Claim First Login
                var isFirstLoginClaim = User.FindFirst("FirstLogin");
                if (isFirstLoginClaim != null)
                {

                    var identity = (ClaimsIdentity)User.Identity;
                    identity.RemoveClaim(isFirstLoginClaim);

                    var updatedPrincipal = new ClaimsPrincipal(identity);

                    // Sign in with the updated ClaimsPrincipal
                    await HttpContext.SignInAsync(updatedPrincipal);
                }
                return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["Message"] = result.Errors.FirstOrDefault()?.Description;
                return RedirectToAction("ChangePasswordFirstLogin");
            }
        }
    }
}
