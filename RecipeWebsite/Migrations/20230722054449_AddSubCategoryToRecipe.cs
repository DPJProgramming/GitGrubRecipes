using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecipeWebsite.Migrations
{
    public partial class AddSubCategoryToRecipe : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SubCategory",
                table: "Recipe",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SubCategory",
                table: "Recipe");
        }
    }
}
