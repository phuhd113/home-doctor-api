using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HomeDoctor.Data.Migrations
{
    public partial class updateDB38 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VitalSignSchedule_VitalSignType_VitalSignTypeId",
                table: "VitalSignSchedule");

            migrationBuilder.DropIndex(
                name: "IX_VitalSignSchedule_MedicalInstructionId",
                table: "VitalSignSchedule");

            migrationBuilder.DropIndex(
                name: "IX_VitalSignSchedule_VitalSignTypeId",
                table: "VitalSignSchedule");

            migrationBuilder.DropColumn(
                name: "MinuteAgain",
                table: "VitalSignSchedule");

            migrationBuilder.DropColumn(
                name: "MinuteDangerInterval",
                table: "VitalSignSchedule");

            migrationBuilder.DropColumn(
                name: "NumberMax",
                table: "VitalSignSchedule");

            migrationBuilder.DropColumn(
                name: "NumberMin",
                table: "VitalSignSchedule");

            migrationBuilder.DropColumn(
                name: "VitalSignTypeId",
                table: "VitalSignSchedule");

            migrationBuilder.RenameColumn(
                name: "TimeStart",
                table: "VitalSignSchedule",
                newName: "Status");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateFinished",
                table: "VitalSignSchedule",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateStarted",
                table: "VitalSignSchedule",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "VitalSign",
                columns: table => new
                {
                    VitalSignId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NumberMax = table.Column<int>(type: "int", nullable: true),
                    NumberMin = table.Column<int>(type: "int", nullable: true),
                    MinuteDangerInterval = table.Column<int>(type: "int", nullable: true),
                    TimeStart = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MinuteAgain = table.Column<int>(type: "int", nullable: true),
                    VitalSignTypeId = table.Column<int>(type: "int", nullable: false),
                    VitalSignScheduleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VitalSign", x => x.VitalSignId);
                    table.ForeignKey(
                        name: "FK_VitalSign_VitalSignSchedule_VitalSignScheduleId",
                        column: x => x.VitalSignScheduleId,
                        principalTable: "VitalSignSchedule",
                        principalColumn: "VitalSignScheduleId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VitalSign_VitalSignType_VitalSignTypeId",
                        column: x => x.VitalSignTypeId,
                        principalTable: "VitalSignType",
                        principalColumn: "VitalSignTypeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VitalSignSchedule_MedicalInstructionId",
                table: "VitalSignSchedule",
                column: "MedicalInstructionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_VitalSign_VitalSignScheduleId",
                table: "VitalSign",
                column: "VitalSignScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_VitalSign_VitalSignTypeId",
                table: "VitalSign",
                column: "VitalSignTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VitalSign");

            migrationBuilder.DropIndex(
                name: "IX_VitalSignSchedule_MedicalInstructionId",
                table: "VitalSignSchedule");

            migrationBuilder.DropColumn(
                name: "DateFinished",
                table: "VitalSignSchedule");

            migrationBuilder.DropColumn(
                name: "DateStarted",
                table: "VitalSignSchedule");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "VitalSignSchedule",
                newName: "TimeStart");

            migrationBuilder.AddColumn<int>(
                name: "MinuteAgain",
                table: "VitalSignSchedule",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MinuteDangerInterval",
                table: "VitalSignSchedule",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NumberMax",
                table: "VitalSignSchedule",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NumberMin",
                table: "VitalSignSchedule",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "VitalSignTypeId",
                table: "VitalSignSchedule",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_VitalSignSchedule_MedicalInstructionId",
                table: "VitalSignSchedule",
                column: "MedicalInstructionId");

            migrationBuilder.CreateIndex(
                name: "IX_VitalSignSchedule_VitalSignTypeId",
                table: "VitalSignSchedule",
                column: "VitalSignTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_VitalSignSchedule_VitalSignType_VitalSignTypeId",
                table: "VitalSignSchedule",
                column: "VitalSignTypeId",
                principalTable: "VitalSignType",
                principalColumn: "VitalSignTypeId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
