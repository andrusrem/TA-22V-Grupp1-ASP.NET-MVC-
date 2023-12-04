using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KooliProjekt.Migrations
{
    /// <inheritdoc />
    public partial class TimeAddedToOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetails_AspNetUsers_CustomerId",
                table: "OrderDetails");

            migrationBuilder.DropIndex(
                name: "IX_OrderDetails_CustomerId",
                table: "OrderDetails");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "OrderDetails");

            migrationBuilder.RenameColumn(
                name: "ProductName",
                table: "OrderDetails",
                newName: "Quantity");

            migrationBuilder.RenameColumn(
                name: "ProductEstimatedPrice",
                table: "OrderDetails",
                newName: "UnitPrice");

            migrationBuilder.AddColumn<DateTime>(
                name: "WhenTaken",
                table: "Orders",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "WhenTaken",
                table: "Myorders",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WhenTaken",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "WhenTaken",
                table: "Myorders");

            migrationBuilder.RenameColumn(
                name: "UnitPrice",
                table: "OrderDetails",
                newName: "ProductEstimatedPrice");

            migrationBuilder.RenameColumn(
                name: "Quantity",
                table: "OrderDetails",
                newName: "ProductName");

            migrationBuilder.AddColumn<string>(
                name: "CustomerId",
                table: "OrderDetails",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_CustomerId",
                table: "OrderDetails",
                column: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetails_AspNetUsers_CustomerId",
                table: "OrderDetails",
                column: "CustomerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
