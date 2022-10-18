using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookingSoccers.Repo.Migrations
{
    public partial class Update2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Users_ReceiverInfoId",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Payments_ReceiverInfoId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "ReceiverInfoId",
                table: "Payments");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_ReceiverId",
                table: "Payments",
                column: "ReceiverId");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Users_ReceiverId",
                table: "Payments",
                column: "ReceiverId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Users_ReceiverId",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Payments_ReceiverId",
                table: "Payments");

            migrationBuilder.AddColumn<int>(
                name: "ReceiverInfoId",
                table: "Payments",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Payments_ReceiverInfoId",
                table: "Payments",
                column: "ReceiverInfoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Users_ReceiverInfoId",
                table: "Payments",
                column: "ReceiverInfoId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
