using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KooliProjekt.Migrations
{
    /// <inheritdoc />
    public partial class Update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EstimatedPrice",
                table: "Orders",
                newName: "ProductEstimatedPrice");

            migrationBuilder.RenameColumn(
                name: "UnitPrice",
                table: "OrderDetails",
                newName: "ProductEstimatedPrice");

            migrationBuilder.RenameColumn(
                name: "Quantity",
                table: "OrderDetails",
                newName: "ProductName");

            migrationBuilder.RenameColumn(
                name: "EstimatedPrice",
                table: "Myorders",
                newName: "ProductEstimatedPrice");

            migrationBuilder.AddColumn<string>(
                name: "CustomerId",
                table: "OrderDetails",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProductName",
                table: "Myorders",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "myinvoices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ProductId = table.Column<int>(type: "INTEGER", nullable: false),
                    ProductName = table.Column<string>(type: "TEXT", nullable: true),
                    WhenTaken = table.Column<DateTime>(type: "TEXT", nullable: false),
                    GivenBack = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DistanceDriven = table.Column<decimal>(type: "TEXT", nullable: false),
                    TotalPrice = table.Column<decimal>(type: "TEXT", nullable: false),
                    PayBy = table.Column<DateTime>(type: "TEXT", nullable: false),
                    PayStatus = table.Column<bool>(type: "INTEGER", nullable: false),
                    CustomerId = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_myinvoices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_myinvoices_AspNetUsers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_myinvoices_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_CustomerId",
                table: "OrderDetails",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_myinvoices_CustomerId",
                table: "myinvoices",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_myinvoices_ProductId",
                table: "myinvoices",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetails_AspNetUsers_CustomerId",
                table: "OrderDetails",
                column: "CustomerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetails_AspNetUsers_CustomerId",
                table: "OrderDetails");

            migrationBuilder.DropTable(
                name: "myinvoices");

            migrationBuilder.DropIndex(
                name: "IX_OrderDetails_CustomerId",
                table: "OrderDetails");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "OrderDetails");

            migrationBuilder.DropColumn(
                name: "ProductName",
                table: "Myorders");

            migrationBuilder.RenameColumn(
                name: "ProductEstimatedPrice",
                table: "Orders",
                newName: "EstimatedPrice");

            migrationBuilder.RenameColumn(
                name: "ProductName",
                table: "OrderDetails",
                newName: "Quantity");

            migrationBuilder.RenameColumn(
                name: "ProductEstimatedPrice",
                table: "OrderDetails",
                newName: "UnitPrice");

            migrationBuilder.RenameColumn(
                name: "ProductEstimatedPrice",
                table: "Myorders",
                newName: "EstimatedPrice");
        }
    }
}
