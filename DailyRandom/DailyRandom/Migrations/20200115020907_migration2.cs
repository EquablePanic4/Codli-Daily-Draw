using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DailyRandom.Migrations
{
    public partial class migration2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ApplicationClients",
                columns: table => new
                {
                    applicationClientId = table.Column<Guid>(nullable: false),
                    ClientDescription = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationClients", x => x.applicationClientId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationClients");
        }
    }
}
