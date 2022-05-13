using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace LinkedHU_CENG.Migrations
{
    public partial class a13 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Certificates");

            migrationBuilder.AddColumn<string>(
                name: "Certificate1Path",
                table: "Applications",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Certificate2Path",
                table: "Applications",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Certificate3Path",
                table: "Applications",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserProfilePicture",
                table: "Applications",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Advertisements",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "Resources",
                columns: table => new
                {
                    ResourceId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: true),
                    Title = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<string>(type: "text", nullable: false),
                    ResourceImageName = table.Column<string>(type: "text", nullable: true),
                    ResourceVideoName = table.Column<string>(type: "text", nullable: true),
                    ResourcePdfName = table.Column<string>(type: "text", nullable: true),
                    ResourceWordName = table.Column<string>(type: "text", nullable: true),
                    ResourceExelName = table.Column<string>(type: "text", nullable: true),
                    ResourcePointName = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Resources", x => x.ResourceId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Resources");

            migrationBuilder.DropColumn(
                name: "Certificate1Path",
                table: "Applications");

            migrationBuilder.DropColumn(
                name: "Certificate2Path",
                table: "Applications");

            migrationBuilder.DropColumn(
                name: "Certificate3Path",
                table: "Applications");

            migrationBuilder.DropColumn(
                name: "UserProfilePicture",
                table: "Applications");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Advertisements");

            migrationBuilder.CreateTable(
                name: "Certificates",
                columns: table => new
                {
                    CertificateId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ApplicationId = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<string>(type: "text", nullable: false),
                    FilePath = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Certificates", x => x.CertificateId);
                });
        }
    }
}
