using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace pos.Migrations
{
	/// <inheritdoc />
	public partial class CreateTable : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.AddColumn<bool>(
				name: "Active",
				table: "AspNetUsers",
				type: "bit",
				nullable: false,
				defaultValue: false);

			migrationBuilder.AddColumn<string>(
				name: "Avatar",
				table: "AspNetUsers",
				type: "nvarchar(max)",
				nullable: false,
				defaultValue: "");

			migrationBuilder.AddColumn<bool>(
				name: "FirstLogin",
				table: "AspNetUsers",
				type: "bit",
				nullable: false,
				defaultValue: false);

			migrationBuilder.AddColumn<string>(
				name: "Gender",
				table: "AspNetUsers",
				type: "nvarchar(max)",
				nullable: false,
				defaultValue: "");

			migrationBuilder.CreateTable(
				name: "Categories",
				columns: table => new
				{
					Id = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Categories", x => x.Id);
				});

			migrationBuilder.CreateTable(
				name: "Customer",
				columns: table => new
				{
					Id = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
					PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
					Address = table.Column<string>(type: "nvarchar(max)", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Customer", x => x.Id);
				});

			migrationBuilder.CreateTable(
				name: "RetailStores",
				columns: table => new
				{
					Id = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					StoreName = table.Column<string>(type: "nvarchar(max)", nullable: false),
					Location = table.Column<string>(type: "nvarchar(max)", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_RetailStores", x => x.Id);
				});

			migrationBuilder.CreateTable(
				name: "Products",
				columns: table => new
				{
					Id = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
					Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
					Cost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
					Barcode = table.Column<string>(type: "nvarchar(450)", nullable: false),
					Quantity = table.Column<int>(type: "int", nullable: false),
					ImagePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
					CategoryId = table.Column<int>(type: "int", nullable: true),
					isDelete = table.Column<bool>(type: "bit", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Products", x => x.Id);
					table.ForeignKey(
						name: "FK_Products_Categories_CategoryId",
						column: x => x.CategoryId,
						principalTable: "Categories",
						principalColumn: "Id",
						onDelete: ReferentialAction.SetNull);
				});

			migrationBuilder.CreateTable(
				name: "Orders",
				columns: table => new
				{
					Id = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					OrderId = table.Column<string>(type: "nvarchar(max)", nullable: false),
					Total = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
					CustomerId = table.Column<int>(type: "int", nullable: false),
					UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
					RetailStoreId = table.Column<int>(type: "int", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Orders", x => x.Id);
					table.ForeignKey(
						name: "FK_Orders_AspNetUsers_UserId",
						column: x => x.UserId,
						principalTable: "AspNetUsers",
						principalColumn: "Id");
					table.ForeignKey(
						name: "FK_Orders_Customer_CustomerId",
						column: x => x.CustomerId,
						principalTable: "Customer",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
					table.ForeignKey(
						name: "FK_Orders_RetailStores_RetailStoreId",
						column: x => x.RetailStoreId,
						principalTable: "RetailStores",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "Inventory",
				columns: table => new
				{
					Id = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					Quantity = table.Column<int>(type: "int", nullable: false),
					RetailSotreId = table.Column<int>(type: "int", nullable: false),
					ProductId = table.Column<int>(type: "int", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Inventory", x => x.Id);
					table.ForeignKey(
						name: "FK_Inventory_Products_ProductId",
						column: x => x.ProductId,
						principalTable: "Products",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
					table.ForeignKey(
						name: "FK_Inventory_RetailStores_RetailSotreId",
						column: x => x.RetailSotreId,
						principalTable: "RetailStores",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "OrderDetails",
				columns: table => new
				{
					Id = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					Subtotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
					Quantity = table.Column<int>(type: "int", nullable: false),
					OrderId = table.Column<int>(type: "int", nullable: false),
					ProductId = table.Column<int>(type: "int", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_OrderDetails", x => x.Id);
					table.ForeignKey(
						name: "FK_OrderDetails_Orders_OrderId",
						column: x => x.OrderId,
						principalTable: "Orders",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
					table.ForeignKey(
						name: "FK_OrderDetails_Products_ProductId",
						column: x => x.ProductId,
						principalTable: "Products",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.InsertData(
				table: "AspNetRoles",
				columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
				values: new object[,]
				{
					{ "cd96adde-26d0-4d5f-81d4-4e0126c1e066", "1", "Manager", "Employee" },
					{ "d92a0c33-28c8-4d73-980e-93c3c97ac592", "1", "Admin", "Admin" }
				});

			migrationBuilder.InsertData(
				table: "AspNetUsers",
				columns: new[] { "Id", "AccessFailedCount", "Active", "Avatar", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstLogin", "Gender", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
				values: new object[] { "5e42a903-0701-4c95-ba0d-4190615f6581", 0, true, "~/images/default/profile/user-1.png", "e2ff9400-d412-47ba-a3f2-c7c07595a353", "admin@gmail.com", true, false, "Male", false, null, null, null, "$2a$10$RXi.j9p8VlMBqzkJMUz5pe2n/cfEtkp00pV9obhD1mbLt7sTIFOuG", null, false, "80202c57-7547-4f03-9c32-c1ada72c9e76", false, "admin" });

			migrationBuilder.CreateIndex(
				name: "IX_Inventory_ProductId",
				table: "Inventory",
				column: "ProductId");

			migrationBuilder.CreateIndex(
				name: "IX_Inventory_RetailSotreId",
				table: "Inventory",
				column: "RetailSotreId");

			migrationBuilder.CreateIndex(
				name: "IX_OrderDetails_OrderId",
				table: "OrderDetails",
				column: "OrderId");

			migrationBuilder.CreateIndex(
				name: "IX_OrderDetails_ProductId",
				table: "OrderDetails",
				column: "ProductId");

			migrationBuilder.CreateIndex(
				name: "IX_Orders_CustomerId",
				table: "Orders",
				column: "CustomerId");

			migrationBuilder.CreateIndex(
				name: "IX_Orders_RetailStoreId",
				table: "Orders",
				column: "RetailStoreId");

			migrationBuilder.CreateIndex(
				name: "IX_Orders_UserId",
				table: "Orders",
				column: "UserId");

			migrationBuilder.CreateIndex(
				name: "IX_Products_Barcode",
				table: "Products",
				column: "Barcode",
				unique: true);

			migrationBuilder.CreateIndex(
				name: "IX_Products_CategoryId",
				table: "Products",
				column: "CategoryId");
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				name: "Inventory");

			migrationBuilder.DropTable(
				name: "OrderDetails");

			migrationBuilder.DropTable(
				name: "Orders");

			migrationBuilder.DropTable(
				name: "Products");

			migrationBuilder.DropTable(
				name: "Customer");

			migrationBuilder.DropTable(
				name: "RetailStores");

			migrationBuilder.DropTable(
				name: "Categories");

			migrationBuilder.DeleteData(
				table: "AspNetRoles",
				keyColumn: "Id",
				keyValue: "cd96adde-26d0-4d5f-81d4-4e0126c1e066");

			migrationBuilder.DeleteData(
				table: "AspNetRoles",
				keyColumn: "Id",
				keyValue: "d92a0c33-28c8-4d73-980e-93c3c97ac592");

			migrationBuilder.DeleteData(
				table: "AspNetUsers",
				keyColumn: "Id",
				keyValue: "5e42a903-0701-4c95-ba0d-4190615f6581");

			migrationBuilder.DropColumn(
				name: "Active",
				table: "AspNetUsers");

			migrationBuilder.DropColumn(
				name: "Avatar",
				table: "AspNetUsers");

			migrationBuilder.DropColumn(
				name: "FirstLogin",
				table: "AspNetUsers");

			migrationBuilder.DropColumn(
				name: "Gender",
				table: "AspNetUsers");
		}
	}
}
