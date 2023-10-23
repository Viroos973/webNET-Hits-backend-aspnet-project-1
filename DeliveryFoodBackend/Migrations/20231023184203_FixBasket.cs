using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DeliveryFoodBackend.Migrations
{
    /// <inheritdoc />
    public partial class FixBasket : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BasketDish");

            migrationBuilder.DropIndex(
                name: "IX_Baskets_UserId_OrderId",
                table: "Baskets");

            migrationBuilder.AddColumn<Guid>(
                name: "DishId",
                table: "Baskets",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Baskets_DishId",
                table: "Baskets",
                column: "DishId");

            migrationBuilder.CreateIndex(
                name: "IX_Baskets_UserId_OrderId_DishId",
                table: "Baskets",
                columns: new[] { "UserId", "OrderId", "DishId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Baskets_Dishes_DishId",
                table: "Baskets",
                column: "DishId",
                principalTable: "Dishes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Baskets_Dishes_DishId",
                table: "Baskets");

            migrationBuilder.DropIndex(
                name: "IX_Baskets_DishId",
                table: "Baskets");

            migrationBuilder.DropIndex(
                name: "IX_Baskets_UserId_OrderId_DishId",
                table: "Baskets");

            migrationBuilder.DropColumn(
                name: "DishId",
                table: "Baskets");

            migrationBuilder.CreateTable(
                name: "BasketDish",
                columns: table => new
                {
                    BasketsId = table.Column<Guid>(type: "uuid", nullable: false),
                    DishesId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BasketDish", x => new { x.BasketsId, x.DishesId });
                    table.ForeignKey(
                        name: "FK_BasketDish_Baskets_BasketsId",
                        column: x => x.BasketsId,
                        principalTable: "Baskets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BasketDish_Dishes_DishesId",
                        column: x => x.DishesId,
                        principalTable: "Dishes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Baskets_UserId_OrderId",
                table: "Baskets",
                columns: new[] { "UserId", "OrderId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BasketDish_DishesId",
                table: "BasketDish",
                column: "DishesId");
        }
    }
}
