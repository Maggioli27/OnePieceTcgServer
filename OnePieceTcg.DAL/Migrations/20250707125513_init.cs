using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnePieceTcg.DAL.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cards_SpecialRarity_SpecialRarityId",
                table: "Cards");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SpecialRarity",
                table: "SpecialRarity");

            migrationBuilder.RenameTable(
                name: "SpecialRarity",
                newName: "SpecialRarities");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SpecialRarities",
                table: "SpecialRarities",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Cards_SpecialRarities_SpecialRarityId",
                table: "Cards",
                column: "SpecialRarityId",
                principalTable: "SpecialRarities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cards_SpecialRarities_SpecialRarityId",
                table: "Cards");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SpecialRarities",
                table: "SpecialRarities");

            migrationBuilder.RenameTable(
                name: "SpecialRarities",
                newName: "SpecialRarity");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SpecialRarity",
                table: "SpecialRarity",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Cards_SpecialRarity_SpecialRarityId",
                table: "Cards",
                column: "SpecialRarityId",
                principalTable: "SpecialRarity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
