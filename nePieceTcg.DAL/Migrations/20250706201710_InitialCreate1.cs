using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnePieceTcg.DAL.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SpecialRarityId",
                table: "Cards",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "SpecialRarity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpecialRarity", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cards_SpecialRarityId",
                table: "Cards",
                column: "SpecialRarityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cards_SpecialRarity_SpecialRarityId",
                table: "Cards",
                column: "SpecialRarityId",
                principalTable: "SpecialRarity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cards_SpecialRarity_SpecialRarityId",
                table: "Cards");

            migrationBuilder.DropTable(
                name: "SpecialRarity");

            migrationBuilder.DropIndex(
                name: "IX_Cards_SpecialRarityId",
                table: "Cards");

            migrationBuilder.DropColumn(
                name: "SpecialRarityId",
                table: "Cards");
        }
    }
}
