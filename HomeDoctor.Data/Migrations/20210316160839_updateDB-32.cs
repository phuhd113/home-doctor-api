using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HomeDoctor.Data.Migrations
{
    public partial class updateDB32 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MedicationSchedule_MedicalInstruction_MedicalInstructionId",
                table: "MedicationSchedule");

            migrationBuilder.DropColumn(
                name: "DateFinished",
                table: "MedicalInstruction");

            migrationBuilder.DropColumn(
                name: "DateStarted",
                table: "MedicalInstruction");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "MedicalInstruction");

            migrationBuilder.RenameColumn(
                name: "MedicalInstructionId",
                table: "MedicationSchedule",
                newName: "PrescriptionId");

            migrationBuilder.RenameIndex(
                name: "IX_MedicationSchedule_MedicalInstructionId",
                table: "MedicationSchedule",
                newName: "IX_MedicationSchedule_PrescriptionId");

            migrationBuilder.CreateTable(
                name: "Prescription",
                columns: table => new
                {
                    PrescriptionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateStarted = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateFinished = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateCanceled = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReasonCancel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MedicalInstructionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prescription", x => x.PrescriptionId);
                    table.ForeignKey(
                        name: "FK_Prescription_MedicalInstruction_MedicalInstructionId",
                        column: x => x.MedicalInstructionId,
                        principalTable: "MedicalInstruction",
                        principalColumn: "MedicalInstructionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Prescription_MedicalInstructionId",
                table: "Prescription",
                column: "MedicalInstructionId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_MedicationSchedule_Prescription_PrescriptionId",
                table: "MedicationSchedule",
                column: "PrescriptionId",
                principalTable: "Prescription",
                principalColumn: "PrescriptionId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MedicationSchedule_Prescription_PrescriptionId",
                table: "MedicationSchedule");

            migrationBuilder.DropTable(
                name: "Prescription");

            migrationBuilder.RenameColumn(
                name: "PrescriptionId",
                table: "MedicationSchedule",
                newName: "MedicalInstructionId");

            migrationBuilder.RenameIndex(
                name: "IX_MedicationSchedule_PrescriptionId",
                table: "MedicationSchedule",
                newName: "IX_MedicationSchedule_MedicalInstructionId");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateFinished",
                table: "MedicalInstruction",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateStarted",
                table: "MedicalInstruction",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "MedicalInstruction",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_MedicationSchedule_MedicalInstruction_MedicalInstructionId",
                table: "MedicationSchedule",
                column: "MedicalInstructionId",
                principalTable: "MedicalInstruction",
                principalColumn: "MedicalInstructionId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
