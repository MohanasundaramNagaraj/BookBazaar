using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookBazaar.Migrations
{
    /// <inheritdoc />
    public partial class MSN_30JUN2024_03 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
             name: "ProductId",
             table: "ShoppingCarts",
             newName: "BookId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
             name: "BookId",
             table: "ShoppingCarts",
             newName: "ProductId");
        }
    }
}
