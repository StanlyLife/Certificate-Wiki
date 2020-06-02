using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Certificate_Wiki.Migrations
{
    public partial class certificates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CertificateContext",
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
                    CertificateUrl = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CertificateContext", x => x.CertificateId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CertificateContext");
        }
    }
}
