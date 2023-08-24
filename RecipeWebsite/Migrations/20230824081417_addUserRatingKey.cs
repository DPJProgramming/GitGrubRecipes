using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecipeWebsite.Migrations
{
    public partial class addUserRatingKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ratingId",
                table: "UserRatings",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserRatings",
                table: "UserRatings",
                column: "ratingId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserRatings",
                table: "UserRatings");

            migrationBuilder.DropColumn(
                name: "ratingId",
                table: "UserRatings");
        }
    }
}
