using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace pos.Migrations
{
    /// <inheritdoc />
    public partial class FixOrderEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_RetailStores_RetailStoreId",
                table: "Orders");

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

            migrationBuilder.AlterColumn<int>(
                name: "RetailStoreId",
                table: "Orders",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1cfa0c09-56b9-4aa2-9ce8-08476366c4e8", "1", "Employee", "Employee" },
                    { "b9d53318-8d19-4bb5-bf62-85625c2c4895", "1", "Manager", "Manager" },
                    { "f9c15b80-1b32-4dbf-9d98-e944d2c47b1d", "1", "Admin", "Admin" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_RetailStores_RetailStoreId",
                table: "Orders",
                column: "RetailStoreId",
                principalTable: "RetailStores",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_RetailStores_RetailStoreId",
                table: "Orders");

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

            migrationBuilder.AlterColumn<int>(
                name: "RetailStoreId",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "3f016d11-90b5-429c-b3ae-c0f15d7d8c08", "1", "Employee", "Employee" },
                    { "52a4938d-d0c8-4701-b19b-8267a79f3e1a", "1", "Admin", "Admin" },
                    { "7ba61067-5dcc-451b-ba60-2b4ecea8d116", "1", "Manager", "Manager" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_RetailStores_RetailStoreId",
                table: "Orders",
                column: "RetailStoreId",
                principalTable: "RetailStores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
