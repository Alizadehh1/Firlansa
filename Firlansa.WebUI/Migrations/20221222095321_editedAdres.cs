using Microsoft.EntityFrameworkCore.Migrations;

namespace Firlansa.WebUI.Migrations
{
    public partial class editedAdres : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Adresses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ParentId",
                table: "Adresses",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Adresses_ParentId",
                table: "Adresses",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Adresses_Adresses_ParentId",
                table: "Adresses",
                column: "ParentId",
                principalTable: "Adresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Adresses_Adresses_ParentId",
                table: "Adresses");

            migrationBuilder.DropIndex(
                name: "IX_Adresses_ParentId",
                table: "Adresses");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "Adresses");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Adresses",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
