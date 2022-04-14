using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LinkedHU_CENG.Migrations
{
    public partial class mig2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "About",
                table: "Users",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "Users",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SecondEmail",
                table: "Users",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "About",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "SecondEmail",
                table: "Users");
        }
    }
}
