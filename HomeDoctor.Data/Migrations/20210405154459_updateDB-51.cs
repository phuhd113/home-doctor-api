using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HomeDoctor.Data.Migrations
{
    public partial class updateDB51 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BloodPressureNumberValue",
                table: "VitalSignValue");

            migrationBuilder.DropColumn(
                name: "BloodPressureTimeValue",
                table: "VitalSignValue");

            migrationBuilder.RenameColumn(
                name: "HeartBeatTimeValue",
                table: "VitalSignValue",
                newName: "TimeValue");

            migrationBuilder.RenameColumn(
                name: "HeartBeatNumberValue",
                table: "VitalSignValue",
                newName: "NumberValue");

            migrationBuilder.AddColumn<int>(
                name: "VitalSignTypeId",
                table: "VitalSignValue",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCanceled",
                table: "VitalSignSchedule",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_VitalSignValue_VitalSignTypeId",
                table: "VitalSignValue",
                column: "VitalSignTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_VitalSignValue_VitalSignType_VitalSignTypeId",
                table: "VitalSignValue",
                column: "VitalSignTypeId",
                principalTable: "VitalSignType",
                principalColumn: "VitalSignTypeId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VitalSignValue_VitalSignType_VitalSignTypeId",
                table: "VitalSignValue");

            migrationBuilder.DropIndex(
                name: "IX_VitalSignValue_VitalSignTypeId",
                table: "VitalSignValue");

            migrationBuilder.DropColumn(
                name: "VitalSignTypeId",
                table: "VitalSignValue");

            migrationBuilder.DropColumn(
                name: "DateCanceled",
                table: "VitalSignSchedule");

            migrationBuilder.RenameColumn(
                name: "TimeValue",
                table: "VitalSignValue",
                newName: "HeartBeatTimeValue");

            migrationBuilder.RenameColumn(
                name: "NumberValue",
                table: "VitalSignValue",
                newName: "HeartBeatNumberValue");

            migrationBuilder.AddColumn<string>(
                name: "BloodPressureNumberValue",
                table: "VitalSignValue",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BloodPressureTimeValue",
                table: "VitalSignValue",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
