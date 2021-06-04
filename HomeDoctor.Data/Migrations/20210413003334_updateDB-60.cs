using Microsoft.EntityFrameworkCore.Migrations;

namespace HomeDoctor.Data.Migrations
{
    public partial class updateDB60 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DiseaseMedicalInstruction");

            migrationBuilder.DropTable(
                name: "MedicalInstructionShareContract");

            migrationBuilder.AddColumn<string>(
                name: "DiseaseId",
                table: "MedicalInstruction",
                type: "nvarchar(450)",
                nullable: true);

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
                name: "IX_MedicalInstruction_DiseaseId",
                table: "MedicalInstruction",
                column: "DiseaseId");

            migrationBuilder.CreateIndex(
                name: "IX_ContractMedicalInstruction_MedicalInstructionsMedicalInstructionId",
                table: "ContractMedicalInstruction",
                column: "MedicalInstructionsMedicalInstructionId");

            migrationBuilder.AddForeignKey(
                name: "FK_MedicalInstruction_Disease_DiseaseId",
                table: "MedicalInstruction",
                column: "DiseaseId",
                principalTable: "Disease",
                principalColumn: "DiseaseId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MedicalInstruction_Disease_DiseaseId",
                table: "MedicalInstruction");

            migrationBuilder.DropTable(
                name: "ContractMedicalInstruction");

            migrationBuilder.DropIndex(
                name: "IX_MedicalInstruction_DiseaseId",
                table: "MedicalInstruction");

            migrationBuilder.DropColumn(
                name: "DiseaseId",
                table: "MedicalInstruction");

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
                name: "IX_DiseaseMedicalInstruction_MedicalInstructionsMedicalInstructionId",
                table: "DiseaseMedicalInstruction",
                column: "MedicalInstructionsMedicalInstructionId");

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
    }
}
