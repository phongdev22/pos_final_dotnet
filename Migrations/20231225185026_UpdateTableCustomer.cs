using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace pos.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTableCustomer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8b032764-aa12-4169-8b78-63c97cc934fa");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c7353729-7d07-4913-aaa0-562462533aa2");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "2dc50d8c-c68f-4efd-98ce-32bf14f33f17", "dc4b1ab6-d55a-45bf-9e95-988311cf8c02" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2dc50d8c-c68f-4efd-98ce-32bf14f33f17");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "dc4b1ab6-d55a-45bf-9e95-988311cf8c02");

            migrationBuilder.AlterColumn<decimal>(
                name: "GivenMoney",
                table: "Orders",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateCreation",
                table: "Orders",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "6e34abf8-b7fb-41f5-ad3b-f6147996c383", "1", "Employee", "Employee" },
                    { "dad4d4d6-b253-4cc6-95d5-cc9e045d8e72", "1", "Admin", "Admin" },
                    { "ff4f9a0e-1cda-444f-b3fb-3def1a8c39ec", "1", "Manager", "Manager" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "Active", "Avatar", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstLogin", "FullName", "Gender", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "RetailStoreId", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "177f555a-64e2-4a64-bc7e-7693bd3f3ff4", 0, true, "/images/default/profile/user-1.png", "e523e07d-91a6-451b-be60-6fd1c5ec486a", "admin@gmail.com", true, false, "Phong Van", true, false, null, null, "admin", "AQAAAAIAAYagAAAAEDYIByDEAzOvY+7I/iK7VYMUnpqI5aIXGbwoilzhYmkzmD8vfe8SIEcchecfxE+c2A==", "0000000000", false, null, "b6432bd6-e20e-4b00-a376-91f3b60f0186", false, "admin" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "dad4d4d6-b253-4cc6-95d5-cc9e045d8e72", "177f555a-64e2-4a64-bc7e-7693bd3f3ff4" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6e34abf8-b7fb-41f5-ad3b-f6147996c383");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ff4f9a0e-1cda-444f-b3fb-3def1a8c39ec");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "dad4d4d6-b253-4cc6-95d5-cc9e045d8e72", "177f555a-64e2-4a64-bc7e-7693bd3f3ff4" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "dad4d4d6-b253-4cc6-95d5-cc9e045d8e72");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "177f555a-64e2-4a64-bc7e-7693bd3f3ff4");

            migrationBuilder.AlterColumn<decimal>(
                name: "GivenMoney",
                table: "Orders",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateCreation",
                table: "Orders",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "2dc50d8c-c68f-4efd-98ce-32bf14f33f17", "1", "Admin", "Admin" },
                    { "8b032764-aa12-4169-8b78-63c97cc934fa", "1", "Employee", "Employee" },
                    { "c7353729-7d07-4913-aaa0-562462533aa2", "1", "Manager", "Manager" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "Active", "Avatar", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstLogin", "FullName", "Gender", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "RetailStoreId", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "dc4b1ab6-d55a-45bf-9e95-988311cf8c02", 0, true, "/images/default/profile/user-1.png", "b2b073ae-21e0-463f-8171-bb3eb3ce27f0", "admin@gmail.com", true, false, "Phong Van", true, false, null, null, "admin", "AQAAAAIAAYagAAAAEIeUSlJ1xV0W4DX4R18fT2KmNEyA3rcoGP8m205uziIGtvpYFQv7dg6rYd0cfoN3hw==", "0000000000", false, null, "c66b7bd2-17b2-4203-ae18-97c1c17734f1", false, "admin" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "2dc50d8c-c68f-4efd-98ce-32bf14f33f17", "dc4b1ab6-d55a-45bf-9e95-988311cf8c02" });
        }
    }
}
