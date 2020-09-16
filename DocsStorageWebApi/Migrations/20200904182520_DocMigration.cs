using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DocsStorageWebApi.Migrations
{
    public partial class DocMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Documents",
                columns: table => new
                {
                    DocumentID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Author = table.Column<string>(nullable: false),
                    DocumentName = table.Column<string>(nullable: false),
                    Exstension = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: false),
                    LoadDate = table.Column<DateTime>(nullable: false),
                    DocumentStorageID = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Documents", x => x.DocumentID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Documents_DocumentStorageID",
                table: "Documents",
                column: "DocumentStorageID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Documents_DocumentID_DocumentName",
                table: "Documents",
                columns: new[] { "DocumentID", "DocumentName" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Documents");
        }
    }
}
