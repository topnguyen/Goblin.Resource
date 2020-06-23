using Microsoft.EntityFrameworkCore.Migrations;

namespace Goblin.Service_Resource.Repository.Migrations
{
    public partial class UpdateFileTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Folder",
                table: "File",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsCompressedImage",
                table: "File",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "StorageFullPath",
                table: "File",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Folder",
                table: "File");

            migrationBuilder.DropColumn(
                name: "IsCompressedImage",
                table: "File");

            migrationBuilder.DropColumn(
                name: "StorageFullPath",
                table: "File");
        }
    }
}
