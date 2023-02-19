using Microsoft.EntityFrameworkCore.Migrations;

namespace Firlansa.WebUI.Migrations
{
    public partial class editteddOrder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Adresses_AdressId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Users_FirlansaUserId1",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_AdressId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_FirlansaUserId1",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "AdressId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "FirlansaUserId1",
                table: "Orders");

            migrationBuilder.AlterColumn<int>(
                name: "FirlansaUserId",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_FirlansaUserId",
                table: "Orders",
                column: "FirlansaUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Users_FirlansaUserId",
                table: "Orders",
                column: "FirlansaUserId",
                principalSchema: "Membership",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Users_FirlansaUserId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_FirlansaUserId",
                table: "Orders");

            migrationBuilder.AlterColumn<string>(
                name: "FirlansaUserId",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "AdressId",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FirlansaUserId1",
                table: "Orders",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_AdressId",
                table: "Orders",
                column: "AdressId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_FirlansaUserId1",
                table: "Orders",
                column: "FirlansaUserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Adresses_AdressId",
                table: "Orders",
                column: "AdressId",
                principalTable: "Adresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Users_FirlansaUserId1",
                table: "Orders",
                column: "FirlansaUserId1",
                principalSchema: "Membership",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
