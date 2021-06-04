using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HomeDoctor.Data.Migrations
{
    public partial class updateDB47 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateApproved",
                table: "Contract",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCancel",
                table: "Contract",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateSigned",
                table: "Contract",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReasonCancel",
                table: "Contract",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateApproved",
                table: "Contract");

            migrationBuilder.DropColumn(
                name: "DateCancel",
                table: "Contract");

            migrationBuilder.DropColumn(
                name: "DateSigned",
                table: "Contract");

            migrationBuilder.DropColumn(
                name: "ReasonCancel",
                table: "Contract");
        }
    }
}
