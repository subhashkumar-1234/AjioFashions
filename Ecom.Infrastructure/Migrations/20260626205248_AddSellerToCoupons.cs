using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecom.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSellerToCoupons : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SellerId",
                table: "Coupons",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Coupons_SellerId",
                table: "Coupons",
                column: "SellerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Coupons_Users_SellerId",
                table: "Coupons",
                column: "SellerId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Coupons_Users_SellerId",
                table: "Coupons");

            migrationBuilder.DropIndex(
                name: "IX_Coupons_SellerId",
                table: "Coupons");

            migrationBuilder.DropColumn(
                name: "SellerId",
                table: "Coupons");
        }
    }
}
