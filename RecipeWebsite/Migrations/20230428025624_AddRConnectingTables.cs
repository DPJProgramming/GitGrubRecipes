using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecipeWebsite.Migrations
{
    public partial class AddRConnectingTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comment_Recipe_RecipeId",
                table: "Comment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Comment",
                table: "Comment");

            migrationBuilder.DropIndex(
                name: "IX_Comment_RecipeId",
                table: "Comment");

            migrationBuilder.DropColumn(
                name: "RecipeId",
                table: "Comment");

            migrationBuilder.RenameTable(
                name: "Comment",
                newName: "Comments");

            migrationBuilder.AddColumn<int>(
                name: "ParentRecipeRecipeId",
                table: "Comments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Comments",
                table: "Comments",
                column: "CommentId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_ParentRecipeRecipeId",
                table: "Comments",
                column: "ParentRecipeRecipeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Recipe_ParentRecipeRecipeId",
                table: "Comments",
                column: "ParentRecipeRecipeId",
                principalTable: "Recipe",
                principalColumn: "RecipeId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Recipe_ParentRecipeRecipeId",
                table: "Comments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Comments",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_ParentRecipeRecipeId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "ParentRecipeRecipeId",
                table: "Comments");

            migrationBuilder.RenameTable(
                name: "Comments",
                newName: "Comment");

            migrationBuilder.AddColumn<int>(
                name: "RecipeId",
                table: "Comment",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Comment",
                table: "Comment",
                column: "CommentId");

            migrationBuilder.CreateIndex(
                name: "IX_Comment_RecipeId",
                table: "Comment",
                column: "RecipeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_Recipe_RecipeId",
                table: "Comment",
                column: "RecipeId",
                principalTable: "Recipe",
                principalColumn: "RecipeId");
        }
    }
}
