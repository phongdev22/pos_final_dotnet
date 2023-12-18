using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using pos.Config;
using pos.Entities;
using pos.Models;
using System.Drawing.Printing;
using System.Net;
using System.Net.Mail;
using pos.Utils;
using System.Net.WebSockets;
using Microsoft.EntityFrameworkCore;

namespace pos.Controllers
{
	public class AccountsController : Controller
	{
		private readonly ApplicationDbContext _context;
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly IConfiguration _configuration;
		private List<IdentityRole> _roles;
		private List<RetailStore> _stores;

		private int PageSize = 10;

		public AccountsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
		{
			_context = context;
			_userManager = userManager;
			_roleManager = roleManager;
			_configuration = configuration;
			_roles = _roleManager.Roles.Where(r => r.Name != "Admin").ToList();
			_stores = _context.RetailStores.ToList();
		}

		public async Task<IActionResult> Index(int page = 1, [FromQuery] int store = 1)
		{
			var currentUserName = User.Identity.Name;
			

			var accounts = _userManager.Users.AsQueryable().Where(c => !c.NormalizedUserName.Equals(currentUserName) && !c.NormalizedUserName.Equals("Admin"))
					.Skip((page - 1) * PageSize)
					.Take(PageSize).ToList();

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
		public async Task<IActionResult> Create(ApplicationUser appUser, [FromForm] string[] Roles)
		{
			// Time at starting create
			var currentUtcTime = DateTimeOffset.Now;

			var listRoles = _roles;

			var username = appUser.Email.Split("@")[0];
			var password = username;

			appUser.NormalizedUserName = username;
			appUser.UserName = username;
			appUser.NormalizedEmail = appUser.Email;

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
					timestamp = currentUtcTime.ToString("yyyyMMddHHmmss")
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
		public async Task<IActionResult> Edit(string id, ApplicationUser appUser, [FromForm] string[] Roles, [FromForm]string password)
		{
			var user = await _userManager.FindByIdAsync(id);

			if (user == null || id != appUser.Id)
			{
				return NotFound();
			}

			// Cập nhật thông tin người dùng từ dữ liệu nhập vào
			user.PhoneNumber = appUser.PhoneNumber;
			user.Email = appUser.Email;
			user.Gender = appUser.Gender;

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
