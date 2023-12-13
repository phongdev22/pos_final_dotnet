﻿using Microsoft.AspNetCore.Identity;
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

		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
		{
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


			SeedingData(builder);
		}

		public static void SeedingData(ModelBuilder builder)
		{
			string hashedPassword = BCrypt.Net.BCrypt.HashPassword("admin", 10);

			builder.Entity<IdentityRole>().HasData(
				new IdentityRole() { Name = "Admin", ConcurrencyStamp = "1", NormalizedName = "Admin" },
				new IdentityRole() { Name = "Manager", ConcurrencyStamp = "1", NormalizedName = "Employee" }
			);

			builder.Entity<ApplicationUser>().HasData(
				new ApplicationUser()
				{
					UserName = "admin",
					NormalizedUserName = "admin",
					Email = "admin@gmail.com",
					NormalizedEmail = "admin@gmail.com",
					PasswordHash = hashedPassword,
					Active = true,
					FirstLogin = false,
					EmailConfirmed = true,
				}
			);
		}
		public DbSet<pos.Entities.Inventory> Inventory { get; set; } = default!;
	}
}
