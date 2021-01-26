using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HomeDoctor.Data.Migrations
{
    public partial class updateDB07 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Experience",
                table: "Doctor",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Specialization",
                table: "Doctor",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AddressPatient",
                table: "Contract",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DOBDoctor",
                table: "Contract",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DOBPatient",
                table: "Contract",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "FullNameDoctor",
                table: "Contract",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FullNamePatient",
                table: "Contract",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumberDoctor",
                table: "Contract",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumberPatient",
                table: "Contract",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WorkLocationDoctor",
                table: "Contract",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Experience",
                table: "Doctor");

            migrationBuilder.DropColumn(
                name: "Specialization",
                table: "Doctor");

            migrationBuilder.DropColumn(
                name: "AddressPatient",
                table: "Contract");

            migrationBuilder.DropColumn(
                name: "DOBDoctor",
                table: "Contract");

            migrationBuilder.DropColumn(
                name: "DOBPatient",
                table: "Contract");

            migrationBuilder.DropColumn(
                name: "FullNameDoctor",
                table: "Contract");

            migrationBuilder.DropColumn(
                name: "FullNamePatient",
                table: "Contract");

            migrationBuilder.DropColumn(
                name: "PhoneNumberDoctor",
                table: "Contract");

            migrationBuilder.DropColumn(
                name: "PhoneNumberPatient",
                table: "Contract");

            migrationBuilder.DropColumn(
                name: "WorkLocationDoctor",
                table: "Contract");
        }
    }
}
