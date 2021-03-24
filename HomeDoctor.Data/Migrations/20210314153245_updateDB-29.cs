using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HomeDoctor.Data.Migrations
{
    public partial class updateDB29 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreate",
                table: "MedicalInstruction",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "MedicalInstruction",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateCreate",
                table: "MedicalInstruction");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "MedicalInstruction");
        }
    }
}
