using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using pos.Entities;

namespace pos.Config
{
	public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
	{
		public DbSet<Customer> Customer { get; set; }
		public DbSet<Product> Products { get; set; }
		public DbSet<Category> Categories { get; set; }
		public DbSet<Order> Orders { get; set; }
		public DbSet<OrderDetail> OrderDetails { get; set; }
		public DbSet<RetailStore> RetailStores { get; set; }
		public DbSet<Inventory> Inventory { get; set; } = default!;


		private readonly UserManager<ApplicationUser> _userManager;

		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
		{
			//_userManager = userManager;
		}

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);

			// Product
			builder.Entity<Product>()
				.HasIndex(p => p.Barcode)
				.IsUnique();

			builder.Entity<Product>()
				.HasMany(p => p.Inventories)
				.WithOne(inv => inv.Product)
				.HasForeignKey(p => p.ProductId)
				.OnDelete(DeleteBehavior.Cascade);

			// Category
			builder.Entity<Category>()
				.HasMany(c => c.Products)
				.WithOne(p => p.Category)
				.HasForeignKey(p => p.CategoryId)
				.OnDelete(DeleteBehavior.SetNull);

			// Order
			builder.Entity<Order>()
				.HasMany(o => o.OrderDetails)
				.WithOne(od => od.Order)
				.HasForeignKey(od => od.OrderId)
				.OnDelete(DeleteBehavior.Cascade);

			// RetailStore
			builder.Entity<RetailStore>()
				.HasMany(rt => rt.Inventories)
				.WithOne(inv => inv.RetailStore)
				.HasForeignKey(inv => inv.RetailStoreId)
				.OnDelete(DeleteBehavior.Cascade);

			builder.Entity<RetailStore>()
				.HasMany(rt => rt.AppUsers)
				.WithOne(us => us.RetailStore)
				.HasForeignKey(us => us.RetailStoreId)
				.OnDelete(DeleteBehavior.Cascade);

			SeedingData(builder);
		}

		public void SeedingData(ModelBuilder builder)
		{

			var roles = new List<IdentityRole>()
			{
				new IdentityRole() { Name = "Admin", ConcurrencyStamp = "1", NormalizedName = "Admin" },
				new IdentityRole() { Name = "Manager", ConcurrencyStamp = "1", NormalizedName = "Manager" },
				new IdentityRole() { Name = "Employee", ConcurrencyStamp = "1", NormalizedName = "Employee" }
			};


			// ROLE
			builder.Entity<IdentityRole>().HasData(roles);

			// USER
			var appUser = new ApplicationUser
			{
				FullName = "Phong Van",
				Email = "admin@gmail.com",
				EmailConfirmed = true,
				UserName = "admin",
				NormalizedUserName = "admin",
				Gender = true,
				PhoneNumber = "0000000000",
				Active = true,
				FirstLogin = false
			};

			PasswordHasher<ApplicationUser> hashedPassword = new PasswordHasher<ApplicationUser>();
			appUser.PasswordHash = hashedPassword.HashPassword(appUser, "admin");

			builder.Entity<ApplicationUser>().HasData(appUser);

			builder.Entity<IdentityUserRole<string>>().HasData(
				new IdentityUserRole<string>() { UserId = appUser.Id, RoleId = roles[0].Id }
			);


		}
	}
}
