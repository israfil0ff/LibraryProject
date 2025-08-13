using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Library.DAL.Migrations
{
    /// <inheritdoc />
    public partial class PendingChangesMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AvailableCount",
                table: "Books",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "ReturnDate",
                table: "BookRentals",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "BookRentals",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_BookRentals_UserId",
                table: "BookRentals",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_BookRentals_Users_UserId",
                table: "BookRentals",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookRentals_Users_UserId",
                table: "BookRentals");

            migrationBuilder.DropIndex(
                name: "IX_BookRentals_UserId",
                table: "BookRentals");

            migrationBuilder.DropColumn(
                name: "AvailableCount",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "ReturnDate",
                table: "BookRentals");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "BookRentals");
        }
    }
}
