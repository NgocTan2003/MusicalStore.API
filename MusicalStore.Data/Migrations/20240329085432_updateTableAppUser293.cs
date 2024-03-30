using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MusicalStore.Data.Migrations
{
    public partial class updateTableAppUser293 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StartTime",
                table: "AspNetUsers",
                newName: "TokenExpirationTime");

            migrationBuilder.RenameColumn(
                name: "ExpiryTime",
                table: "AspNetUsers",
                newName: "RefreshTokenExpirationTime");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TokenExpirationTime",
                table: "AspNetUsers",
                newName: "StartTime");

            migrationBuilder.RenameColumn(
                name: "RefreshTokenExpirationTime",
                table: "AspNetUsers",
                newName: "ExpiryTime");
        }
    }
}
