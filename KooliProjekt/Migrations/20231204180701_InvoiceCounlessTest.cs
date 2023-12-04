using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KooliProjekt.Migrations
{
    /// <inheritdoc />
    public partial class InvoiceCounlessTest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OrderId",
                table: "myinvoices",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<DateTime>(
                name: "WhenTaken",
                table: "Invoices",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<DateTime>(
                name: "GivenBack",
                table: "Invoices",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "TEXT");

            migrationBuilder.AddColumn<int>(
                name: "OrderId",
                table: "Invoices",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_myinvoices_OrderId",
                table: "myinvoices",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_OrderId",
                table: "Invoices",
                column: "OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_Orders_OrderId",
                table: "Invoices",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_myinvoices_Orders_OrderId",
                table: "myinvoices",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_Orders_OrderId",
                table: "Invoices");

            migrationBuilder.DropForeignKey(
                name: "FK_myinvoices_Orders_OrderId",
                table: "myinvoices");

            migrationBuilder.DropIndex(
                name: "IX_myinvoices_OrderId",
                table: "myinvoices");

            migrationBuilder.DropIndex(
                name: "IX_Invoices_OrderId",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "myinvoices");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "Invoices");

            migrationBuilder.AlterColumn<DateTime>(
                name: "WhenTaken",
                table: "Invoices",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "GivenBack",
                table: "Invoices",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldNullable: true);
        }
    }
}
