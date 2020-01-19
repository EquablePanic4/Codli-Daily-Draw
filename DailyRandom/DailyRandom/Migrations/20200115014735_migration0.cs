using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DailyRandom.Migrations
{
    public partial class migration0 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Draws",
                columns: table => new
                {
                    drawId = table.Column<Guid>(nullable: false),
                    date = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Draws", x => x.drawId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    userId = table.Column<Guid>(nullable: false),
                    forname = table.Column<string>(nullable: true),
                    surname = table.Column<string>(nullable: true),
                    drawId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.userId);
                    table.ForeignKey(
                        name: "FK_Users_Draws_drawId",
                        column: x => x.drawId,
                        principalTable: "Draws",
                        principalColumn: "drawId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_drawId",
                table: "Users",
                column: "drawId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Draws");
        }
    }
}
