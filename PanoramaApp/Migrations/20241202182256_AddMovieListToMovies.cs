// <copyright file="20241202182256_AddMovieListToMovies.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

#nullable disable

namespace PanoramaApp.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    /// <inheritdoc />
    public partial class AddMovieListToMovies : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MovieId",
                table: "MovieLists",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MovieLists_MovieId",
                table: "MovieLists",
                column: "MovieId");

            migrationBuilder.AddForeignKey(
                name: "FK_MovieLists_Movies_MovieId",
                table: "MovieLists",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MovieLists_Movies_MovieId",
                table: "MovieLists");

            migrationBuilder.DropIndex(
                name: "IX_MovieLists_MovieId",
                table: "MovieLists");

            migrationBuilder.DropColumn(
                name: "MovieId",
                table: "MovieLists");
        }
    }
}
