using Microsoft.EntityFrameworkCore.Migrations;

namespace HomeDoctor.Data.Migrations
{
    public partial class updateDB59 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContractMedicalInstruction");

            migrationBuilder.CreateTable(
                name: "MedicalInstructionShareContract",
                columns: table => new
                {
                    MedicalInstructionShareContractId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContractId = table.Column<int>(type: "int", nullable: false),
                    DiseaseId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    MedicalInstructionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicalInstructionShareContract", x => x.MedicalInstructionShareContractId);
                    table.ForeignKey(
                        name: "FK_MedicalInstructionShareContract_Contract_ContractId",
                        column: x => x.ContractId,
                        principalTable: "Contract",
                        principalColumn: "ContractId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MedicalInstructionShareContract_Disease_DiseaseId",
                        column: x => x.DiseaseId,
                        principalTable: "Disease",
                        principalColumn: "DiseaseId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MedicalInstructionShareContract_MedicalInstruction_MedicalInstructionId",
                        column: x => x.MedicalInstructionId,
                        principalTable: "MedicalInstruction",
                        principalColumn: "MedicalInstructionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MedicalInstructionShareContract_ContractId",
                table: "MedicalInstructionShareContract",
                column: "ContractId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicalInstructionShareContract_DiseaseId",
                table: "MedicalInstructionShareContract",
                column: "DiseaseId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicalInstructionShareContract_MedicalInstructionId",
                table: "MedicalInstructionShareContract",
                column: "MedicalInstructionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MedicalInstructionShareContract");

            migrationBuilder.CreateTable(
                name: "ContractMedicalInstruction",
                columns: table => new
                {
                    ContractsContractId = table.Column<int>(type: "int", nullable: false),
                    MedicalInstructionsMedicalInstructionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContractMedicalInstruction", x => new { x.ContractsContractId, x.MedicalInstructionsMedicalInstructionId });
                    table.ForeignKey(
                        name: "FK_ContractMedicalInstruction_Contract_ContractsContractId",
                        column: x => x.ContractsContractId,
                        principalTable: "Contract",
                        principalColumn: "ContractId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ContractMedicalInstruction_MedicalInstruction_MedicalInstructionsMedicalInstructionId",
                        column: x => x.MedicalInstructionsMedicalInstructionId,
                        principalTable: "MedicalInstruction",
                        principalColumn: "MedicalInstructionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ContractMedicalInstruction_MedicalInstructionsMedicalInstructionId",
                table: "ContractMedicalInstruction",
                column: "MedicalInstructionsMedicalInstructionId");
        }
    }
}
