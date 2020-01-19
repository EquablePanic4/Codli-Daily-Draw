using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DailyRandom.Migrations
{
    public partial class migration6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Draws_drawId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_drawId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "drawId",
                table: "Users");

            migrationBuilder.AddColumn<List<string>>(
                name: "userIdsOrder",
                table: "Draws",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "userIdsOrder",
                table: "Draws");

            migrationBuilder.AddColumn<Guid>(
                name: "drawId",
                table: "Users",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_drawId",
                table: "Users",
                column: "drawId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Draws_drawId",
                table: "Users",
                column: "drawId",
                principalTable: "Draws",
                principalColumn: "drawId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
