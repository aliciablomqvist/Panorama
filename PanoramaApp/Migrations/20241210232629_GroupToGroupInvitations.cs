// <copyright file="20241210232629_GroupToGroupInvitations.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

#nullable disable

namespace PanoramaApp.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    /// <inheritdoc />
    public partial class GroupToGroupInvitations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "ChatMessages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: string.Empty);

            migrationBuilder.CreateIndex(
                name: "IX_GroupInvitations_GroupId",
                table: "GroupInvitations",
                column: "GroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupInvitations_Groups_GroupId",
                table: "GroupInvitations",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupInvitations_Groups_GroupId",
                table: "GroupInvitations");

            migrationBuilder.DropIndex(
                name: "IX_GroupInvitations_GroupId",
                table: "GroupInvitations");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ChatMessages");
        }
    }
}