using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace pos.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTableInvetory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Inventory_RetailStores_RetailSotreId",
                table: "Inventory");

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

            migrationBuilder.RenameColumn(
                name: "RetailSotreId",
                table: "Inventory",
                newName: "RetailStoreId");

            migrationBuilder.RenameIndex(
                name: "IX_Inventory_RetailSotreId",
                table: "Inventory",
                newName: "IX_Inventory_RetailStoreId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Inventory_RetailStores_RetailStoreId",
                table: "Inventory",
                column: "RetailStoreId",
                principalTable: "RetailStores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Inventory_RetailStores_RetailStoreId",
                table: "Inventory");

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

            migrationBuilder.RenameColumn(
                name: "RetailStoreId",
                table: "Inventory",
                newName: "RetailSotreId");

            migrationBuilder.RenameIndex(
                name: "IX_Inventory_RetailStoreId",
                table: "Inventory",
                newName: "IX_Inventory_RetailSotreId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Inventory_RetailStores_RetailSotreId",
                table: "Inventory",
                column: "RetailSotreId",
                principalTable: "RetailStores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
