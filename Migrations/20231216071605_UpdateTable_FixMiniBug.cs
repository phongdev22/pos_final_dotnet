using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace pos.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTable_FixMiniBug : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1b757f03-60ad-4942-88c3-8024aa545a9a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "576bc9ba-8ca9-4b7f-b35b-7b222db2f24b");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "e4b31b5e-17d3-44f6-b033-ef38ec1310e2");

            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "RetailStoreId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "3f016d11-90b5-429c-b3ae-c0f15d7d8c08", "1", "Employee", "Employee" },
                    { "52a4938d-d0c8-4701-b19b-8267a79f3e1a", "1", "Admin", "Admin" },
                    { "7ba61067-5dcc-451b-ba60-2b4ecea8d116", "1", "Manager", "Manager" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_RetailStoreId",
                table: "AspNetUsers",
                column: "RetailStoreId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_RetailStores_RetailStoreId",
                table: "AspNetUsers",
                column: "RetailStoreId",
                principalTable: "RetailStores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_RetailStores_RetailStoreId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_RetailStoreId",
                table: "AspNetUsers");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3f016d11-90b5-429c-b3ae-c0f15d7d8c08");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "52a4938d-d0c8-4701-b19b-8267a79f3e1a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7ba61067-5dcc-451b-ba60-2b4ecea8d116");

            migrationBuilder.DropColumn(
                name: "FullName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "RetailStoreId",
                table: "AspNetUsers");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1b757f03-60ad-4942-88c3-8024aa545a9a", "1", "Admin", "Admin" },
                    { "576bc9ba-8ca9-4b7f-b35b-7b222db2f24b", "1", "Manager", "Employee" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "Active", "Avatar", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstLogin", "Gender", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "e4b31b5e-17d3-44f6-b033-ef38ec1310e2", 0, true, "~/images/default/profile/user-1.png", "dac77341-7c9d-4a57-a56c-91e9200d15b5", "admin@gmail.com", true, false, "Male", false, null, "admin@gmail.com", "admin", "$2a$10$xJWJ54lFpHtl5uWwj8wCI.6cOMrKWrCC5H1APpw3iUaKZ2cyEhnPC", null, false, "62634abd-f1c8-47ea-92b0-6e34ca8c61c9", false, "admin" });
        }
    }
}
