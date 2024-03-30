using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MusicalStore.Data.Migrations
{
    public partial class UpdateTableUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ExpiryTime",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RefeshToken",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartTime",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExpiryTime",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "RefeshToken",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "StartTime",
                table: "AspNetUsers");
        }
    }
}
