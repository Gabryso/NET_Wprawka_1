using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WA1.Migrations
{
    /// <inheritdoc />
    public partial class OtM2_AddCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "Hero",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Hero_CategoryId",
                table: "Hero",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Hero_Category_CategoryId",
                table: "Hero",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Hero_Category_CategoryId",
                table: "Hero");

            migrationBuilder.DropTable(
                name: "Category");

            migrationBuilder.DropIndex(
                name: "IX_Hero_CategoryId",
                table: "Hero");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Hero");
        }
    }
}
