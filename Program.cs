using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using pos.Config;
using pos.Entities;

namespace pos
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			var configration = builder.Configuration;

			// Add services to the container.
			builder.Services.AddControllersWithViews();

			// DB Configuration
			builder.Services.AddDbContext<ApplicationDbContext>(options =>
			{
				options.UseLazyLoadingProxies().UseSqlServer(configration.GetConnectionString("posSystemDB"));
			});

			// For Identity
			builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
				{
					options.SignIn.RequireConfirmedAccount = false;
					options.Password.RequireNonAlphanumeric = false;
					options.Password.RequireUppercase = false;
					options.Password.RequireLowercase = false;
					options.Password.RequireDigit = false;
					options.Password.RequiredLength = 4;

				})
				.AddEntityFrameworkStores<ApplicationDbContext>()
				.AddDefaultTokenProviders();

			builder.Services.Configure<DataProtectionTokenProviderOptions>(options =>
			{
				options.TokenLifespan = TimeSpan.FromMinutes(1);
			});

			builder.Services.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
				options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
			})
				.AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
				{
					options.LoginPath = "/Auth"; // Đường dẫn đăng nhập của bạn
					options.LogoutPath = "/Auth/Logout";
					options.AccessDeniedPath = "/AccessDenied";
					options.Cookie.HttpOnly = true;
					options.ExpireTimeSpan = TimeSpan.FromDays(1);
					options.SlidingExpiration = true;
				});

			// builder.Services.AddLogging(loggingBuilder => loggingBuilder.AddDebug());

			builder.Services.AddAuthorization();

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (!app.Environment.IsDevelopment())
			{
				app.UseExceptionHandler("/Home/Error");
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();

			app.UseAuthentication();
			app.UseAuthorization();

			//app.UseMiddleware<FirstLoginMiddleware>();
			//app.UseMiddleware<UserInfoMiddleware>();

			app.MapControllerRoute(
				name: "default",
				pattern: "{controller=Home}/{action=Index}/{id?}");

			app.Run();
		}
	}
}
