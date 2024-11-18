using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Library.Migrations
{
    /// <inheritdoc />
    public partial class Cascade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartProductModel_Users_UserId",
                table: "CartProductModel");

            migrationBuilder.AddForeignKey(
                name: "FK_CartProductModel_Users_UserId",
                table: "CartProductModel",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartProductModel_Users_UserId",
                table: "CartProductModel");

            migrationBuilder.AddForeignKey(
                name: "FK_CartProductModel_Users_UserId",
                table: "CartProductModel",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId");
        }
    }
}
