using Microsoft.EntityFrameworkCore.Migrations;

namespace HomeDoctor.Data.Migrations
{
    public partial class updateDB54 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MedicalInstructionShare");

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContractMedicalInstruction");

            migrationBuilder.CreateTable(
                name: "MedicalInstructionShare",
                columns: table => new
                {
                    MedicalInstructionShareId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContractId = table.Column<int>(type: "int", nullable: false),
                    MedicalInstructionId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicalInstructionShare", x => x.MedicalInstructionShareId);
                    table.ForeignKey(
                        name: "FK_MedicalInstructionShare_Contract_ContractId",
                        column: x => x.ContractId,
                        principalTable: "Contract",
                        principalColumn: "ContractId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MedicalInstructionShare_MedicalInstruction_MedicalInstructionId",
                        column: x => x.MedicalInstructionId,
                        principalTable: "MedicalInstruction",
                        principalColumn: "MedicalInstructionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MedicalInstructionShare_ContractId",
                table: "MedicalInstructionShare",
                column: "ContractId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicalInstructionShare_MedicalInstructionId",
                table: "MedicalInstructionShare",
                column: "MedicalInstructionId");
        }
    }
}
