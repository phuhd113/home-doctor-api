using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HomeDoctor.Data.Migrations
{
    public partial class updateDB53 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActionEveryDay");

            migrationBuilder.DropTable(
                name: "BloodPressure");

            migrationBuilder.DropTable(
                name: "ActionEveryWeek");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "MedicalInstruction");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "HealthRecord",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MedicalInstructionImage",
                columns: table => new
                {
                    MedicalInstructionImageId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MedicalInstructionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicalInstructionImage", x => x.MedicalInstructionImageId);
                    table.ForeignKey(
                        name: "FK_MedicalInstructionImage_MedicalInstruction_MedicalInstructionId",
                        column: x => x.MedicalInstructionId,
                        principalTable: "MedicalInstruction",
                        principalColumn: "MedicalInstructionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MedicalInstructionImage_MedicalInstructionId",
                table: "MedicalInstructionImage",
                column: "MedicalInstructionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MedicalInstructionImage");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "HealthRecord");

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "MedicalInstruction",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ActionEveryWeek",
                columns: table => new
                {
                    ActionEveryWeekId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ActionFirstTimeId = table.Column<int>(type: "int", nullable: true),
                    AppointmentWeek = table.Column<bool>(type: "bit", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PrescriptionWeek = table.Column<bool>(type: "bit", nullable: false),
                    VitalSignWeek = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActionEveryWeek", x => x.ActionEveryWeekId);
                    table.ForeignKey(
                        name: "FK_ActionEveryWeek_ActionFirstTime_ActionFirstTimeId",
                        column: x => x.ActionFirstTimeId,
                        principalTable: "ActionFirstTime",
                        principalColumn: "ActionFirstTimeId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BloodPressure",
                columns: table => new
                {
                    BloodPressureId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BloodPressureRate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TimeCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    VitalSignId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BloodPressure", x => x.BloodPressureId);
                    table.ForeignKey(
                        name: "FK_BloodPressure_VitalSign_VitalSignId",
                        column: x => x.VitalSignId,
                        principalTable: "VitalSign",
                        principalColumn: "VitalSignId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ActionEveryDay",
                columns: table => new
                {
                    ActionEveryDayId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ActionEveryWeekId = table.Column<int>(type: "int", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Examination = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActionEveryDay", x => x.ActionEveryDayId);
                    table.ForeignKey(
                        name: "FK_ActionEveryDay_ActionEveryWeek_ActionEveryWeekId",
                        column: x => x.ActionEveryWeekId,
                        principalTable: "ActionEveryWeek",
                        principalColumn: "ActionEveryWeekId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActionEveryDay_ActionEveryWeekId",
                table: "ActionEveryDay",
                column: "ActionEveryWeekId");

            migrationBuilder.CreateIndex(
                name: "IX_ActionEveryWeek_ActionFirstTimeId",
                table: "ActionEveryWeek",
                column: "ActionFirstTimeId");

            migrationBuilder.CreateIndex(
                name: "IX_BloodPressure_VitalSignId",
                table: "BloodPressure",
                column: "VitalSignId");
        }
    }
}
