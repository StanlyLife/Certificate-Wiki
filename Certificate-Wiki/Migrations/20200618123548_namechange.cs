using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Certificate_Wiki.Migrations
{
    public partial class namechange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Certificates",
                columns: table => new
                {
                    CertificateId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserFk = table.Column<string>(nullable: true),
                    CertificateName = table.Column<string>(nullable: true),
                    Subject = table.Column<string>(nullable: true),
                    Admissioner = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    CertificateFile = table.Column<byte[]>(nullable: true),
                    CertificateFileName = table.Column<string>(nullable: true),
                    CertificateUrl = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Certificates", x => x.CertificateId);
                });

            migrationBuilder.CreateTable(
                name: "FavoriteCertificates",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    certificateId = table.Column<int>(nullable: false),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FavoriteCertificates", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Certificates");

            migrationBuilder.DropTable(
                name: "FavoriteCertificates");
        }
    }
}
