using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Library.Migrations
{
    /// <inheritdoc />
    public partial class MajorChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartProductModel_ShoppingCartModel_ShoppingCartId",
                table: "CartProductModel");

            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingCartHistoryModel_ShoppingCartModel_ShoppingCartId",
                table: "ShoppingCartHistoryModel");

            migrationBuilder.DropTable(
                name: "ShoppingCartModel");

            migrationBuilder.DropIndex(
                name: "IX_ShoppingCartHistoryModel_ShoppingCartId",
                table: "ShoppingCartHistoryModel");

            migrationBuilder.DropIndex(
                name: "IX_CartProductModel_ProductId_ShoppingCartId",
                table: "CartProductModel");

            migrationBuilder.DropColumn(
                name: "ShoppingCartId",
                table: "ShoppingCartHistoryModel");

            migrationBuilder.RenameColumn(
                name: "ShoppingCartId",
                table: "CartProductModel",
                newName: "ShoppingCartHistoryModelId");

            migrationBuilder.RenameIndex(
                name: "IX_CartProductModel_ShoppingCartId",
                table: "CartProductModel",
                newName: "IX_CartProductModel_ShoppingCartHistoryModelId");

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalPrice",
                table: "ShoppingCartHistoryModel",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AddColumn<int>(
                name: "PaymentMethod",
                table: "ShoppingCartHistoryModel",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CartProductModel_ProductId",
                table: "CartProductModel",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_CartProductModel_ShoppingCartHistoryModel_ShoppingCartHistoryModelId",
                table: "CartProductModel",
                column: "ShoppingCartHistoryModelId",
                principalTable: "ShoppingCartHistoryModel",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartProductModel_ShoppingCartHistoryModel_ShoppingCartHistoryModelId",
                table: "CartProductModel");

            migrationBuilder.DropIndex(
                name: "IX_CartProductModel_ProductId",
                table: "CartProductModel");

            migrationBuilder.DropColumn(
                name: "PaymentMethod",
                table: "ShoppingCartHistoryModel");

            migrationBuilder.RenameColumn(
                name: "ShoppingCartHistoryModelId",
                table: "CartProductModel",
                newName: "ShoppingCartId");

            migrationBuilder.RenameIndex(
                name: "IX_CartProductModel_ShoppingCartHistoryModelId",
                table: "CartProductModel",
                newName: "IX_CartProductModel_ShoppingCartId");

            migrationBuilder.AlterColumn<double>(
                name: "TotalPrice",
                table: "ShoppingCartHistoryModel",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldPrecision: 18,
                oldScale: 2);

            migrationBuilder.AddColumn<int>(
                name: "ShoppingCartId",
                table: "ShoppingCartHistoryModel",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ShoppingCartModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    IsPaid = table.Column<bool>(type: "bit", nullable: false),
                    PaymentMethod = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShoppingCartModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShoppingCartModel_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingCartHistoryModel_ShoppingCartId",
                table: "ShoppingCartHistoryModel",
                column: "ShoppingCartId");

            migrationBuilder.CreateIndex(
                name: "IX_CartProductModel_ProductId_ShoppingCartId",
                table: "CartProductModel",
                columns: new[] { "ProductId", "ShoppingCartId" },
                unique: true,
                filter: "[ShoppingCartId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingCartModel_UserId",
                table: "ShoppingCartModel",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_CartProductModel_ShoppingCartModel_ShoppingCartId",
                table: "CartProductModel",
                column: "ShoppingCartId",
                principalTable: "ShoppingCartModel",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingCartHistoryModel_ShoppingCartModel_ShoppingCartId",
                table: "ShoppingCartHistoryModel",
                column: "ShoppingCartId",
                principalTable: "ShoppingCartModel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
