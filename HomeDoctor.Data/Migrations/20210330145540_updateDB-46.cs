using Microsoft.EntityFrameworkCore.Migrations;

namespace HomeDoctor.Data.Migrations
{
    public partial class updateDB46 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VitalSignSchedule_MedicalInstruction_MedicalInstructionId",
                table: "VitalSignSchedule");

            migrationBuilder.DropIndex(
                name: "IX_VitalSignSchedule_MedicalInstructionId",
                table: "VitalSignSchedule");

            migrationBuilder.DropColumn(
                name: "MedicalInstructionId",
                table: "VitalSignSchedule");

            migrationBuilder.AddColumn<int>(
                name: "VitalSignScheduleId",
                table: "MedicalInstruction",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MedicalInstruction_VitalSignScheduleId",
                table: "MedicalInstruction",
                column: "VitalSignScheduleId");

            migrationBuilder.AddForeignKey(
                name: "FK_MedicalInstruction_VitalSignSchedule_VitalSignScheduleId",
                table: "MedicalInstruction",
                column: "VitalSignScheduleId",
                principalTable: "VitalSignSchedule",
                principalColumn: "VitalSignScheduleId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MedicalInstruction_VitalSignSchedule_VitalSignScheduleId",
                table: "MedicalInstruction");

            migrationBuilder.DropIndex(
                name: "IX_MedicalInstruction_VitalSignScheduleId",
                table: "MedicalInstruction");

            migrationBuilder.DropColumn(
                name: "VitalSignScheduleId",
                table: "MedicalInstruction");

            migrationBuilder.AddColumn<int>(
                name: "MedicalInstructionId",
                table: "VitalSignSchedule",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_VitalSignSchedule_MedicalInstructionId",
                table: "VitalSignSchedule",
                column: "MedicalInstructionId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_VitalSignSchedule_MedicalInstruction_MedicalInstructionId",
                table: "VitalSignSchedule",
                column: "MedicalInstructionId",
                principalTable: "MedicalInstruction",
                principalColumn: "MedicalInstructionId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
