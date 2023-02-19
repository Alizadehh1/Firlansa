using Microsoft.EntityFrameworkCore.Migrations;

namespace Firlansa.WebUI.Migrations
{
    public partial class productStatusId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductStatuses",
                table: "ProductStatuses");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "ProductStatuses",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "MyProperty",
                table: "ProductStatuses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductStatuses",
                table: "ProductStatuses",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_ProductStatuses_ProductId",
                table: "ProductStatuses",
                column: "ProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductStatuses",
                table: "ProductStatuses");

            migrationBuilder.DropIndex(
                name: "IX_ProductStatuses_ProductId",
                table: "ProductStatuses");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "ProductStatuses");

            migrationBuilder.DropColumn(
                name: "MyProperty",
                table: "ProductStatuses");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductStatuses",
                table: "ProductStatuses",
                columns: new[] { "ProductId", "SizeId", "ColorId", "FirlansaUserId" });
        }
    }
}
