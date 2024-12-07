using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PanoramaApp.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMovieModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           /* migrationBuilder.DropColumn(
                name: "Id",
                table: "MovieListItems");*/

           /* migrationBuilder.DropColumn(
                name: "Title",
                table: "MovieListItems");*/

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Movies",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Genre",
                table: "Movies",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "ReleaseDate",
                table: "Movies",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Movies");

            migrationBuilder.DropColumn(
                name: "Genre",
                table: "Movies");

            migrationBuilder.DropColumn(
                name: "ReleaseDate",
                table: "Movies");

          /*  migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "MovieListItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "MovieListItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");*/
        }
    }
}
