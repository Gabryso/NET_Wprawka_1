using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WA1.Migrations
{
    /// <inheritdoc />
    public partial class MtM_AddMission : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Mission",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mission", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HeroMission",
                columns: table => new
                {
                    HeroesId = table.Column<int>(type: "int", nullable: false),
                    MissionsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HeroMission", x => new { x.HeroesId, x.MissionsId });
                    table.ForeignKey(
                        name: "FK_HeroMission_Hero_HeroesId",
                        column: x => x.HeroesId,
                        principalTable: "Hero",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HeroMission_Mission_MissionsId",
                        column: x => x.MissionsId,
                        principalTable: "Mission",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HeroMission_MissionsId",
                table: "HeroMission",
                column: "MissionsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HeroMission");

            migrationBuilder.DropTable(
                name: "Mission");
        }
    }
}
