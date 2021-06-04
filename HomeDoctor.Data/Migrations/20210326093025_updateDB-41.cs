using Microsoft.EntityFrameworkCore.Migrations;

namespace HomeDoctor.Data.Migrations
{
    public partial class updateDB41 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Prescription_MedicalInstruction_MedicalInstructionId",
                table: "Prescription");

            migrationBuilder.DropIndex(
                name: "IX_Prescription_MedicalInstructionId",
                table: "Prescription");

            migrationBuilder.DropColumn(
                name: "MedicalInstructionId",
                table: "Prescription");

            migrationBuilder.AddColumn<int>(
                name: "PrescriptionId",
                table: "MedicalInstruction",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "MedicalInstruction",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MedicalInstruction_PrescriptionId",
                table: "MedicalInstruction",
                column: "PrescriptionId");

            migrationBuilder.AddForeignKey(
                name: "FK_MedicalInstruction_Prescription_PrescriptionId",
                table: "MedicalInstruction",
                column: "PrescriptionId",
                principalTable: "Prescription",
                principalColumn: "PrescriptionId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MedicalInstruction_Prescription_PrescriptionId",
                table: "MedicalInstruction");

            migrationBuilder.DropIndex(
                name: "IX_MedicalInstruction_PrescriptionId",
                table: "MedicalInstruction");

            migrationBuilder.DropColumn(
                name: "PrescriptionId",
                table: "MedicalInstruction");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "MedicalInstruction");

            migrationBuilder.AddColumn<int>(
                name: "MedicalInstructionId",
                table: "Prescription",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Prescription_MedicalInstructionId",
                table: "Prescription",
                column: "MedicalInstructionId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Prescription_MedicalInstruction_MedicalInstructionId",
                table: "Prescription",
                column: "MedicalInstructionId",
                principalTable: "MedicalInstruction",
                principalColumn: "MedicalInstructionId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
