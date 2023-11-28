using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlazingCuisine.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddCategoryPhoto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Photo",
                table: "Categories",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Photo",
                table: "Categories");
        }
    }
}
