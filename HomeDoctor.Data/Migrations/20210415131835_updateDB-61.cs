using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HomeDoctor.Data.Migrations
{
    public partial class updateDB61 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointment_MedicalInstruction_MedicalInstructionId",
                table: "Appointment");

            migrationBuilder.DropIndex(
                name: "IX_Appointment_MedicalInstructionId",
                table: "Appointment");

            migrationBuilder.RenameColumn(
                name: "PrescriptionFirst",
                table: "HealthRecord",
                newName: "VitalSignScheduleFirst");

            migrationBuilder.RenameColumn(
                name: "MedicalInstructionId",
                table: "Appointment",
                newName: "HealthRecordId");

            migrationBuilder.AddColumn<int>(
                name: "AppointmentId",
                table: "MedicalInstruction",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "Appointment",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Diagnose",
                table: "Appointment",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MedicalInstruction_AppointmentId",
                table: "MedicalInstruction",
                column: "AppointmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointment_HealthRecordId",
                table: "Appointment",
                column: "HealthRecordId");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointment_HealthRecord_HealthRecordId",
                table: "Appointment",
                column: "HealthRecordId",
                principalTable: "HealthRecord",
                principalColumn: "HealthRecordId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MedicalInstruction_Appointment_AppointmentId",
                table: "MedicalInstruction",
                column: "AppointmentId",
                principalTable: "Appointment",
                principalColumn: "AppointmentId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointment_HealthRecord_HealthRecordId",
                table: "Appointment");

            migrationBuilder.DropForeignKey(
                name: "FK_MedicalInstruction_Appointment_AppointmentId",
                table: "MedicalInstruction");

            migrationBuilder.DropIndex(
                name: "IX_MedicalInstruction_AppointmentId",
                table: "MedicalInstruction");

            migrationBuilder.DropIndex(
                name: "IX_Appointment_HealthRecordId",
                table: "Appointment");

            migrationBuilder.DropColumn(
                name: "AppointmentId",
                table: "MedicalInstruction");

            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "Appointment");

            migrationBuilder.DropColumn(
                name: "Diagnose",
                table: "Appointment");

            migrationBuilder.RenameColumn(
                name: "VitalSignScheduleFirst",
                table: "HealthRecord",
                newName: "PrescriptionFirst");

            migrationBuilder.RenameColumn(
                name: "HealthRecordId",
                table: "Appointment",
                newName: "MedicalInstructionId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointment_MedicalInstructionId",
                table: "Appointment",
                column: "MedicalInstructionId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Appointment_MedicalInstruction_MedicalInstructionId",
                table: "Appointment",
                column: "MedicalInstructionId",
                principalTable: "MedicalInstruction",
                principalColumn: "MedicalInstructionId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
