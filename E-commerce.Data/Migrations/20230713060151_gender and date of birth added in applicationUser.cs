using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_commerce.Data.Migrations
{
    /// <inheritdoc />
    public partial class genderanddateofbirthaddedinapplicationUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Gender",
                table: "AspNetUsers",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "dob",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "ID",
                keyValue: 1,
                columns: new[] { "createdAt", "updatedAt" },
                values: new object[] { "7/13/2023 11:46:51 AM", "7/13/2023 11:46:51 AM" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 1,
                columns: new[] { "createdAt", "updatedAt" },
                values: new object[] { "7/13/2023 11:46:51 AM", "7/13/2023 11:46:51 AM" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Gender",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "dob",
                table: "AspNetUsers");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "ID",
                keyValue: 1,
                columns: new[] { "createdAt", "updatedAt" },
                values: new object[] { "7/11/2023 1:41:53 PM", "7/11/2023 1:41:53 PM" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 1,
                columns: new[] { "createdAt", "updatedAt" },
                values: new object[] { "7/11/2023 1:41:53 PM", "7/11/2023 1:41:53 PM" });
        }
    }
}
