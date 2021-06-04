using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HomeDoctor.Data.Migrations
{
    public partial class updateDB43 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateUpdateStatus",
                table: "PersonalHealthRecord",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MIShareFromId",
                table: "MedicalInstruction",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateUpdateStatus",
                table: "PersonalHealthRecord");

            migrationBuilder.DropColumn(
                name: "MIShareFromId",
                table: "MedicalInstruction");
        }
    }
}
