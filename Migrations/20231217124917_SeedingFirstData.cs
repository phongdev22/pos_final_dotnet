using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace pos.Migrations
{
    /// <inheritdoc />
    public partial class SeedingFirstData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
    }
}
