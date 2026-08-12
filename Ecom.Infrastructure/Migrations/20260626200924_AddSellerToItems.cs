using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecom.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSellerToItems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SellerId",
                table: "AddItems",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AddItems_SellerId",
                table: "AddItems",
                column: "SellerId");

            migrationBuilder.AddForeignKey(
                name: "FK_AddItems_Users_SellerId",
                table: "AddItems",
                column: "SellerId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AddItems_Users_SellerId",
                table: "AddItems");

            migrationBuilder.DropIndex(
                name: "IX_AddItems_SellerId",
                table: "AddItems");

            migrationBuilder.DropColumn(
                name: "SellerId",
                table: "AddItems");
        }
    }
}
