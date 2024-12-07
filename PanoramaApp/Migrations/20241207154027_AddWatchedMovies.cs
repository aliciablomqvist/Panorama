using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PanoramaApp.Migrations
{
    /// <inheritdoc />
    public partial class AddWatchedMovies : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "MovieListItems",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Id",
                table: "MovieListItems");
        }
    }
}
