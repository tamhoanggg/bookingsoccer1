using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookingSoccers.Repo.Migrations
{
    public partial class update3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ImageFolders_FieldId",
                table: "ImageFolders");

            migrationBuilder.CreateIndex(
                name: "IX_ImageFolders_FieldId",
                table: "ImageFolders",
                column: "FieldId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ImageFolders_FieldId",
                table: "ImageFolders");

            migrationBuilder.CreateIndex(
                name: "IX_ImageFolders_FieldId",
                table: "ImageFolders",
                column: "FieldId",
                unique: true);
        }
    }
}
