using Microsoft.EntityFrameworkCore.Migrations;

namespace HomeDoctor.Data.Migrations
{
    public partial class updateDB58 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointment_Contract_ContractId",
                table: "Appointment");

            migrationBuilder.DropIndex(
                name: "IX_Appointment_ContractId",
                table: "Appointment");

            migrationBuilder.DropColumn(
                name: "ContractId",
                table: "Appointment");

            migrationBuilder.CreateTable(
                name: "DiseaseMedicalInstruction",
                columns: table => new
                {
                    DiseasesDiseaseId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MedicalInstructionsMedicalInstructionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiseaseMedicalInstruction", x => new { x.DiseasesDiseaseId, x.MedicalInstructionsMedicalInstructionId });
                    table.ForeignKey(
                        name: "FK_DiseaseMedicalInstruction_Disease_DiseasesDiseaseId",
                        column: x => x.DiseasesDiseaseId,
                        principalTable: "Disease",
                        principalColumn: "DiseaseId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DiseaseMedicalInstruction_MedicalInstruction_MedicalInstructionsMedicalInstructionId",
                        column: x => x.MedicalInstructionsMedicalInstructionId,
                        principalTable: "MedicalInstruction",
                        principalColumn: "MedicalInstructionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DiseaseMedicalInstruction_MedicalInstructionsMedicalInstructionId",
                table: "DiseaseMedicalInstruction",
                column: "MedicalInstructionsMedicalInstructionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DiseaseMedicalInstruction");

            migrationBuilder.AddColumn<int>(
                name: "ContractId",
                table: "Appointment",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Appointment_ContractId",
                table: "Appointment",
                column: "ContractId");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointment_Contract_ContractId",
                table: "Appointment",
                column: "ContractId",
                principalTable: "Contract",
                principalColumn: "ContractId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
