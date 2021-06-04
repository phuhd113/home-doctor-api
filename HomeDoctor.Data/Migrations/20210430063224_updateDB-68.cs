using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HomeDoctor.Data.Migrations
{
    public partial class updateDB68 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateActive",
                table: "License",
                type: "datetime",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCancel",
                table: "License",
                type: "datetime",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FromBy",
                table: "License",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateActive",
                table: "License");

            migrationBuilder.DropColumn(
                name: "DateCancel",
                table: "License");

            migrationBuilder.DropColumn(
                name: "FromBy",
                table: "License");
        }
    }
}
