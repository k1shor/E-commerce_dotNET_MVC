using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_commerce.Data.Migrations
{
    /// <inheritdoc />
    public partial class sessionIdadded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SessionId",
                table: "OrderHeader",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "ID",
                keyValue: 1,
                columns: new[] { "createdAt", "updatedAt" },
                values: new object[] { "8/7/2023 1:10:29 PM", "8/7/2023 1:10:29 PM" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 1,
                columns: new[] { "createdAt", "updatedAt" },
                values: new object[] { "8/7/2023 1:10:29 PM", "8/7/2023 1:10:29 PM" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SessionId",
                table: "OrderHeader");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "ID",
                keyValue: 1,
                columns: new[] { "createdAt", "updatedAt" },
                values: new object[] { "7/30/2023 11:51:21 AM", "7/30/2023 11:51:21 AM" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 1,
                columns: new[] { "createdAt", "updatedAt" },
                values: new object[] { "7/30/2023 11:51:21 AM", "7/30/2023 11:51:21 AM" });
        }
    }
}
