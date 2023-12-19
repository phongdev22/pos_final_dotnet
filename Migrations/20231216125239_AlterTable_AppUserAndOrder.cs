using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace pos.Migrations
{
	/// <inheritdoc />
	public partial class AlterTable_AppUserAndOrder : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DeleteData(
				table: "AspNetRoles",
				keyColumn: "Id",
				keyValue: "1cfa0c09-56b9-4aa2-9ce8-08476366c4e8");

			migrationBuilder.DeleteData(
				table: "AspNetRoles",
				keyColumn: "Id",
				keyValue: "b9d53318-8d19-4bb5-bf62-85625c2c4895");

			migrationBuilder.DeleteData(
				table: "AspNetRoles",
				keyColumn: "Id",
				keyValue: "f9c15b80-1b32-4dbf-9d98-e944d2c47b1d");

			migrationBuilder.AddColumn<DateTime>(
				name: "DateCreation",
				table: "Orders",
				type: "datetime2",
				nullable: true);

			migrationBuilder.AddColumn<decimal>(
				name: "GivenMoney",
				table: "Orders",
				type: "decimal(18,2)",
				nullable: true);

			migrationBuilder.AddColumn<bool>(
				name: "Status",
				table: "Orders",
				type: "bit",
				nullable: false,
				defaultValue: false);

			migrationBuilder.AlterColumn<bool>(
				name: "Gender",
				table: "AspNetUsers",
				type: "bit",
				nullable: false,
				oldClrType: typeof(string),
				oldType: "nvarchar(max)");

			migrationBuilder.InsertData(
				table: "AspNetRoles",
				columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
				values: new object[,]
				{
					{ "82e5a96d-c597-44b6-8da1-45821aa83c79", "1", "Admin", "Admin" },
					{ "8531ed3b-11cb-40f1-84f4-e34e70235b29", "1", "Manager", "Manager" },
					{ "9da216da-376e-4e63-a64a-7a4730d461c2", "1", "Employee", "Employee" }
				});

		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DeleteData(
				table: "AspNetRoles",
				keyColumn: "Id",
				keyValue: "82e5a96d-c597-44b6-8da1-45821aa83c79");

			migrationBuilder.DeleteData(
				table: "AspNetRoles",
				keyColumn: "Id",
				keyValue: "8531ed3b-11cb-40f1-84f4-e34e70235b29");

			migrationBuilder.DeleteData(
				table: "AspNetRoles",
				keyColumn: "Id",
				keyValue: "9da216da-376e-4e63-a64a-7a4730d461c2");

			migrationBuilder.DropColumn(
				name: "DateCreation",
				table: "Orders");

			migrationBuilder.DropColumn(
				name: "GivenMoney",
				table: "Orders");

			migrationBuilder.DropColumn(
				name: "Status",
				table: "Orders");

			migrationBuilder.AlterColumn<string>(
				name: "Gender",
				table: "AspNetUsers",
				type: "nvarchar(max)",
				nullable: false,
				oldClrType: typeof(bool),
				oldType: "bit");

			migrationBuilder.InsertData(
				table: "AspNetRoles",
				columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
				values: new object[,]
				{
					{ "1cfa0c09-56b9-4aa2-9ce8-08476366c4e8", "1", "Employee", "Employee" },
					{ "b9d53318-8d19-4bb5-bf62-85625c2c4895", "1", "Manager", "Manager" },
					{ "f9c15b80-1b32-4dbf-9d98-e944d2c47b1d", "1", "Admin", "Admin" }
				});
		}
	}
}
