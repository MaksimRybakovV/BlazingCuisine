using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlazingCuisine.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddRecipeOwner : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Owner",
                table: "Recipes",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Owner",
                table: "Recipes");
        }
    }
}
