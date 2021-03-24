using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HomeDoctor.Data.Migrations
{
    public partial class updateDB11 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NumberOfExamination",
                table: "License",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "MedicalInstructionType",
                columns: table => new
                {
                    MedicalInstructionTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicalInstructionType", x => x.MedicalInstructionTypeId);
                });

            migrationBuilder.CreateTable(
                name: "PersonalHealthRecord",
                columns: table => new
                {
                    PersonalHealthRecordId = table.Column<int>(type: "int", nullable: false),
                    PersonalMedicalHistory = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FamilyMedicalHistory = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonalHealthRecord", x => x.PersonalHealthRecordId);
                    table.ForeignKey(
                        name: "FK_PersonalHealthRecord_Patient_PersonalHealthRecordId",
                        column: x => x.PersonalHealthRecordId,
                        principalTable: "Patient",
                        principalColumn: "PatientId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HealthRecord",
                columns: table => new
                {
                    HealthRecordId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Dicease = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Place = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PersonalHealthRecordId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HealthRecord", x => x.HealthRecordId);
                    table.ForeignKey(
                        name: "FK_HealthRecord_PersonalHealthRecord_PersonalHealthRecordId",
                        column: x => x.PersonalHealthRecordId,
                        principalTable: "PersonalHealthRecord",
                        principalColumn: "PersonalHealthRecordId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MedicalInstruction",
                columns: table => new
                {
                    MedicalInstructionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Diagnose = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateStarted = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateFinished = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MedicalInstructionTypeId = table.Column<int>(type: "int", nullable: false),
                    HealthRecordId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicalInstruction", x => x.MedicalInstructionId);
                    table.ForeignKey(
                        name: "FK_MedicalInstruction_HealthRecord_HealthRecordId",
                        column: x => x.HealthRecordId,
                        principalTable: "HealthRecord",
                        principalColumn: "HealthRecordId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MedicalInstruction_MedicalInstructionType_MedicalInstructionTypeId",
                        column: x => x.MedicalInstructionTypeId,
                        principalTable: "MedicalInstructionType",
                        principalColumn: "MedicalInstructionTypeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MedicationSchedule",
                columns: table => new
                {
                    MedicationScheduleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MedicationName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UseTime = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Unit = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<int>(type: "int", nullable: false),
                    Morning = table.Column<bool>(type: "bit", nullable: false),
                    Noon = table.Column<bool>(type: "bit", nullable: false),
                    Night = table.Column<bool>(type: "bit", nullable: false),
                    MedicalInstructionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicationSchedule", x => x.MedicationScheduleId);
                    table.ForeignKey(
                        name: "FK_MedicationSchedule_MedicalInstruction_MedicalInstructionId",
                        column: x => x.MedicalInstructionId,
                        principalTable: "MedicalInstruction",
                        principalColumn: "MedicalInstructionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VitalSignSchedule",
                columns: table => new
                {
                    VitalSignScheduleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MedicalInstructionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VitalSignSchedule", x => x.VitalSignScheduleId);
                    table.ForeignKey(
                        name: "FK_VitalSignSchedule_MedicalInstruction_MedicalInstructionId",
                        column: x => x.MedicalInstructionId,
                        principalTable: "MedicalInstruction",
                        principalColumn: "MedicalInstructionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HealthRecord_PersonalHealthRecordId",
                table: "HealthRecord",
                column: "PersonalHealthRecordId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicalInstruction_HealthRecordId",
                table: "MedicalInstruction",
                column: "HealthRecordId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicalInstruction_MedicalInstructionTypeId",
                table: "MedicalInstruction",
                column: "MedicalInstructionTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicationSchedule_MedicalInstructionId",
                table: "MedicationSchedule",
                column: "MedicalInstructionId");

            migrationBuilder.CreateIndex(
                name: "IX_VitalSignSchedule_MedicalInstructionId",
                table: "VitalSignSchedule",
                column: "MedicalInstructionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MedicationSchedule");

            migrationBuilder.DropTable(
                name: "VitalSignSchedule");

            migrationBuilder.DropTable(
                name: "MedicalInstruction");

            migrationBuilder.DropTable(
                name: "HealthRecord");

            migrationBuilder.DropTable(
                name: "MedicalInstructionType");

            migrationBuilder.DropTable(
                name: "PersonalHealthRecord");

            migrationBuilder.DropColumn(
                name: "NumberOfExamination",
                table: "License");
        }
    }
}
