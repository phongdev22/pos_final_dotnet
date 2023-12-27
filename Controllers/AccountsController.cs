using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using pos.Config;
using pos.Entities;
using pos.Models;
using pos.Utils;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

namespace pos.Controllers
{
	public class AccountsController : Controller
	{
		private readonly IWebHostEnvironment _hostingEnvironment;
		private readonly IConfiguration _configuration;
		private readonly ApplicationDbContext _context;
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;
		private List<IdentityRole> _roles;
		private List<RetailStore> _stores;

		private int PageSize = 10;

		public AccountsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, IWebHostEnvironment hostingEnvironment)
		{
			_context = context;
			_userManager = userManager;
			_roleManager = roleManager;
			_configuration = configuration;
			_roles = _roleManager.Roles.Where(r => r.Name != "Admin").ToList();
			_stores = _context.RetailStores.ToList();
			_hostingEnvironment = hostingEnvironment;
		}

		public async Task<IActionResult> Index(int page = 1, [FromQuery] int store = 1)
		{
			var currentUserName = User.Identity.Name;

			var accounts = new List<ApplicationUser>();

			if (User.IsInRole("Manager"))
			{

				var currentUser = await _userManager.FindByNameAsync(currentUserName);

				if (currentUser != null)
				{
					accounts = (await _userManager.GetUsersInRoleAsync("Employee")).Where(acc => acc.RetailStoreId == currentUser.RetailStoreId).Skip((page - 1) * PageSize)
						.Take(PageSize).ToList();
					accounts.Remove(currentUser);
				}

			}

			if (User.IsInRole("Admin"))
			{
				accounts = await _userManager.Users.AsQueryable().Where(c => !c.NormalizedUserName.Equals(currentUserName) && !c.NormalizedUserName.Equals("Admin"))
					.Skip((page - 1) * PageSize)
					.Take(PageSize).ToListAsync();
			}

			var pageAccount = new PageViewModel<ApplicationUser>() { Items = accounts, PageNumber = page, PageSize = PageSize, TotalItems = accounts.Count };
			return View(pageAccount);
		}

		public IActionResult Create()
		{
			ViewBag.Stores = _stores;
			ViewBag.Roles = _roles;
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Create(ApplicationUser appUser, [FromForm] string[] Roles, int retailId)
		{
			// Time at starting create
			var currentUtcTime = DateTimeOffset.Now;

			var listRoles = _roles;
			ViewBag.Roles = listRoles;

			var store = _context.RetailStores.FirstOrDefault(retail => retail.Id == retailId);

			var username = appUser.Email.Split("@")[0];
			var password = username;

			appUser.NormalizedUserName = username;
			appUser.UserName = username;
			appUser.NormalizedEmail = appUser.Email;
			appUser.RetailStore = store;

			var account = await _userManager.CreateAsync(appUser, password);

			if (account.Succeeded)
			{
				foreach (var role in Roles)
				{
					await _userManager.AddToRoleAsync(appUser, role);
				}

				var emailConfirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(appUser);

				var confirmationLink = Url.Action("Verify", "Auth", new
				{
					userId = appUser.Id,
					token = emailConfirmationToken,
				}, Request.Scheme);

				var emailSubject = "Welcome to YourApp";
				var emailContent = @"Thank you for registering with YourApp. Your account has been created successfully." +
				   $" Please confirm your email by clicking <a href='{confirmationLink}'>here</a>.";

				await Helpers.SendEmail(_configuration, appUser.Email, emailSubject, emailContent);

				return RedirectToAction("Index");
			}
			else
			{
				var duplicateEmailError = account.Errors.FirstOrDefault(error => error.Code == "DuplicateEmail");

				if (duplicateEmailError != null)
				{
					ModelState.AddModelError(string.Empty, duplicateEmailError.Description);
				}
				else
				{
					ModelState.AddModelError(string.Empty, account.Errors.FirstOrDefault()?.Description);
				}

				return View(appUser);
			}
		}

		public async Task<IActionResult> Edit(string id)
		{
			ViewBag.Roles = _roles;
			ViewBag.Stores = _stores;

			if (id == null)
			{
				return NotFound();
			}

			var user = await _userManager.FindByIdAsync(id);

			if (user == null)
			{
				return NotFound();
			}

			var roles = await _userManager.GetRolesAsync(user);
			ViewBag.UserRoles = roles.ToList();

			return View(user);
		}

		[HttpPost]
		public async Task<IActionResult> Edit(string id, ApplicationUser appUser, [FromForm] string[] Roles, [FromForm] string password, [FromForm]int retailId)
		{
			var user = await _userManager.FindByIdAsync(id);

			if (user == null || id != appUser.Id)
			{
				return NotFound();
			}

			user.PhoneNumber = appUser.PhoneNumber;
			user.Email = appUser.Email;
			user.Gender = appUser.Gender;
			user.RetailStoreId = retailId;

			if (!string.IsNullOrEmpty(password))
			{
				var token = await _userManager.GeneratePasswordResetTokenAsync(user);
				var result = await _userManager.ResetPasswordAsync(user, token, password);

				if (!result.Succeeded)
				{
					return RedirectToAction("Index");
				}
			}
			// Update Roles
			await UpdateUserRoles(user, Roles);

			// Cập nhật người dùng trong cơ sở dữ liệu
			var updateResult = await _userManager.UpdateAsync(user);

			if (updateResult.Succeeded)
			{
				return RedirectToAction("Index");
			}

			return RedirectToAction("Edit");
		}

		[HttpPost]
		public async Task<IActionResult> ChangeStatus([FromQuery] string id)
		{
			var user = await _userManager.FindByIdAsync(id);

			if (user == null) return Ok(new { code = 1, Message = "Cannot change Failed!" });

			user.Active = !user.Active;

			var updateResult = await _userManager.UpdateAsync(user);

			if (updateResult.Succeeded)
			{
				return Ok(new { code = 0, Message = "Change Status Success!", status = user.Active });
			}

			return Ok(new { code = 1, Message = "Change Status Failed!" });
		}

		[HttpGet]
		public async Task<IActionResult> Profile(string id)
		{

			ViewBag.Message = TempData["Message"] ?? null;

			var profile = await _userManager.FindByNameAsync(id);

			if (profile == null) return Ok();

			var roles = await _userManager.GetRolesAsync(profile);
			ViewBag.Roles = roles;

			return View(profile);
		}

		[HttpPost]
		public async Task<IActionResult> Profile(ApplicationUser user, IFormFile avatar)
		{

			var current = await _userManager.FindByIdAsync(user.Id);

			if (current == null) { return NotFound(); }


			if (avatar != null)
			{
				if (!string.IsNullOrEmpty(current.Avatar) && !current.Avatar.Equals("/images/default/profile/user-1.png"))
				{
					var oldFilePath = Path.Combine("wwwroot", current.Avatar.TrimStart('/'));

					if (System.IO.File.Exists(oldFilePath))
					{
						System.IO.File.Delete(oldFilePath);
					}
				}

				Helpers.ProcessUpload(avatar, $"{current.Id}.png", Path.Combine("wwwroot", "images", "user"));

				current.Avatar = $"/images/user/{current.Id}.png";

				var existingClaim = User.FindFirst("Avatar");

				// Change avatar when upload 
				if (existingClaim != null)
				{
					var newClaim = new Claim("Avatar", current.Avatar);

					var identity = (ClaimsIdentity)HttpContext.User.Identity;

					identity.RemoveClaim(existingClaim);
					identity.AddClaim(newClaim);

					var principal = new ClaimsPrincipal(identity);
					await HttpContext.SignInAsync(principal);
				}
				// Response.Cookies.Append("AvatarPath", current.Avatar, new CookieOptions() { Expires = DateTime.Now.AddDays(1) });
			}

			current.FullName = user.FullName;
			current.Gender = user.Gender;
			current.PhoneNumber = user.PhoneNumber;
			current.Email = user.Email;

			var result = await _userManager.UpdateAsync(current);

			if (result.Succeeded)
			{
				TempData["Message"] = "Update Profile Success!";
				return RedirectToAction("Profile", new { id = current.UserName });
			}
			else
			{
				TempData["Message"] = "Update Profile Failed!";
				return RedirectToAction("Profile", new { id = current.UserName });
			}

		}

		[HttpPost]
		public async Task<IActionResult> ChangePassword(ApplicationUser updateUser, [FromForm] string oldPassword, [FromForm] string newPassword)
		{
			// Retrieve the current user
			var user = await _userManager.FindByIdAsync(updateUser.Id);

			if (user == null) return Ok();

			// Verify the old password
			var result = await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);

			if (result.Succeeded)
			{
				TempData["Message"] = "Change password Success!";
				return RedirectToAction("Profile", new { id = user.UserName });
			}
			else
			{
				TempData["Message"] = "Change password fail!";
				return RedirectToAction("Profile", new { id = user.UserName });
			}
		}

		[HttpDelete]
		public async Task<IActionResult> Delete(string id)
		{
			var user = await _userManager.FindByIdAsync(id);

			if (user == null) return Ok(new { code = 1, Message = "Not found!" });
			await _userManager.DeleteAsync(user);

			return Ok(new { code = 0, Message = "Success" });
		}

		private async Task UpdateUserRoles(ApplicationUser user, string[] roles)
		{
			var existingRoles = await _userManager.GetRolesAsync(user);
			await _userManager.RemoveFromRolesAsync(user, existingRoles);

			if (roles != null && roles.Length > 0)
			{
				await _userManager.AddToRolesAsync(user, roles);
			}
		}
	}
}
