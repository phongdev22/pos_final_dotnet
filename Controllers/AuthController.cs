﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using pos.Entities;
using pos.Models.Authencation;
using pos.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace pos.Controllers
{
	public class AuthController : Controller
	{
		private readonly SignInManager<ApplicationUser> _signInManager;
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;
		private GenerateToken _generateToken;

		public AuthController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration config)
		{
			_signInManager = signInManager;
			_userManager = userManager;
			_roleManager = roleManager;
			_generateToken = new GenerateToken(config);
		}

		[AllowAnonymous]
		[HttpGet]
		public IActionResult Index()
		{
			ViewData["Title"] = "POS | Login";
			ViewBag.Message = TempData["Message"];
			return View("Login");
		}

		[HttpPost]
		[AllowAnonymous]
		public async Task<IActionResult> Index(LoginView login)
		{
			if (ModelState.IsValid)
			{
				var user = await _userManager.FindByNameAsync(login.Username);
				if (user != null && await _userManager.CheckPasswordAsync(user, login.Password))
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
		[AllowAnonymous]
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