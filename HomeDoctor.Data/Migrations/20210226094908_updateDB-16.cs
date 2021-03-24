using Microsoft.EntityFrameworkCore.Migrations;

namespace HomeDoctor.Data.Migrations
{
    public partial class updateDB16 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MedicalInstructionShare",
                columns: table => new
                {
                    MedicalInstructionShareId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContractId = table.Column<int>(type: "int", nullable: false),
                    MedicalInstructionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicalInstructionShare", x => x.MedicalInstructionShareId);
                    table.ForeignKey(
                        name: "FK_MedicalInstructionShare_Contract_ContractId",
                        column: x => x.ContractId,
                        principalTable: "Contract",
                        principalColumn: "ContractId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MedicalInstructionShare_MedicalInstruction_MedicalInstructionId",
                        column: x => x.MedicalInstructionId,
                        principalTable: "MedicalInstruction",
                        principalColumn: "MedicalInstructionId",
                        onDelete: ReferentialAction.Restrict);
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MedicalInstructionShare");
        }
    }
}
