// <copyright file="20241203202401_ConfigureMovieListRelationships.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

#nullable disable

namespace PanoramaApp.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    /// <inheritdoc />
    public partial class ConfigureMovieListRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Ta bort den befintliga tabellen
            migrationBuilder.DropTable(
                name: "MovieListItems");

            // Skapa tabellen med den nya strukturen
            migrationBuilder.CreateTable(
                name: "MovieListItems",
                columns: table => new
                {
                    MovieListId = table.Column<int>(nullable: false),
                    MovieId = table.Column<int>(nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovieListItems", x => new { x.MovieListId, x.MovieId });
                    table.ForeignKey(
                        name: "FK_MovieListItems_MovieLists_MovieListId",
                        column: x => x.MovieListId,
                        principalTable: "MovieLists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MovieListItems_Movies_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Movies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            // Skapa en index på MovieId om det behövs
            migrationBuilder.CreateIndex(
                name: "IX_MovieListItems_MovieId",
                table: "MovieListItems",
                column: "MovieId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Ta bort den nyskapade tabellen
            migrationBuilder.DropTable(
                name: "MovieListItems");

            // Återskapa den ursprungliga tabellen
            migrationBuilder.CreateTable(
                name: "MovieListItems",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MovieListId = table.Column<int>(nullable: false),
                    MovieId = table.Column<int>(nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovieListItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MovieListItems_MovieLists_MovieListId",
                        column: x => x.MovieListId,
                        principalTable: "MovieLists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MovieListItems_Movies_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Movies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            // Återskapa index på MovieListId
            migrationBuilder.CreateIndex(
                name: "IX_MovieListItems_MovieListId",
                table: "MovieListItems",
                column: "MovieListId");

            // Återskapa index på MovieId
            migrationBuilder.CreateIndex(
                name: "IX_MovieListItems_MovieId",
                table: "MovieListItems",
                column: "MovieId");
        }
    }
}