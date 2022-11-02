using Microsoft.EntityFrameworkCore.Migrations;

namespace Firlansa.WebUI.Migrations
{
    public partial class identityVol2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Adress",
                schema: "Membership",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Name",
                schema: "Membership",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "StoreName",
                schema: "Membership",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Surname",
                schema: "Membership",
                table: "Users");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Adress",
                schema: "Membership",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                schema: "Membership",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "StoreName",
                schema: "Membership",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Surname",
                schema: "Membership",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
