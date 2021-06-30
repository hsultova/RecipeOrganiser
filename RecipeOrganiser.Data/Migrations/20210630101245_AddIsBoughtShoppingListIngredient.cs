using Microsoft.EntityFrameworkCore.Migrations;

namespace RecipeOrganiser.Data.Migrations
{
    public partial class AddIsBoughtShoppingListIngredient : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsBought",
                table: "ShoppingListIngredients",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsBought",
                table: "ShoppingListIngredients");
        }
    }
}
