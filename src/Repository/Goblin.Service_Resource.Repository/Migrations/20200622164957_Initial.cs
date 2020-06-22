using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Goblin.Service_Resource.Repository.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "File",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedTime = table.Column<DateTimeOffset>(nullable: false),
                    LastUpdatedTime = table.Column<DateTimeOffset>(nullable: false),
                    DeletedTime = table.Column<DateTimeOffset>(nullable: true),
                    CreatedBy = table.Column<long>(nullable: true),
                    LastUpdatedBy = table.Column<long>(nullable: true),
                    DeletedBy = table.Column<long>(nullable: true),
                    Slug = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Extension = table.Column<string>(nullable: true),
                    MimeType = table.Column<string>(nullable: true),
                    ContentLength = table.Column<long>(nullable: false),
                    Hash = table.Column<string>(nullable: true),
                    IsImage = table.Column<bool>(nullable: false),
                    ImageDominantHexColor = table.Column<string>(nullable: true),
                    ImageWidthPx = table.Column<int>(nullable: false),
                    ImageHeightPx = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_File", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_File_CreatedTime",
                table: "File",
                column: "CreatedTime");

            migrationBuilder.CreateIndex(
                name: "IX_File_DeletedTime",
                table: "File",
                column: "DeletedTime");

            migrationBuilder.CreateIndex(
                name: "IX_File_Hash",
                table: "File",
                column: "Hash");

            migrationBuilder.CreateIndex(
                name: "IX_File_Id",
                table: "File",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_File_LastUpdatedTime",
                table: "File",
                column: "LastUpdatedTime");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "File");
        }
    }
}
