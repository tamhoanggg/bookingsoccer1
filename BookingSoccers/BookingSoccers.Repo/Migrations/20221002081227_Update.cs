using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookingSoccers.Repo.Migrations
{
    public partial class Update : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Gender",
                table: "Users",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(6)",
                oldMaxLength: 6);

            migrationBuilder.AlterColumn<int>(
                name: "DayType",
                table: "PriceMenus",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(10)",
                oldMaxLength: 10);

            migrationBuilder.AlterColumn<int>(
                name: "Type",
                table: "Payments",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(10)",
                oldMaxLength: 10);

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "Bookings",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(15)",
                oldMaxLength: 15);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Gender",
                table: "Users",
                type: "character varying(6)",
                maxLength: 6,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "DayType",
                table: "PriceMenus",
                type: "character varying(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "Payments",
                type: "character varying(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Bookings",
                type: "character varying(15)",
                maxLength: 15,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");
        }
    }
}
