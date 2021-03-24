using Microsoft.EntityFrameworkCore.Migrations;

namespace HomeDoctor.Data.Migrations
{
    public partial class updateDB26 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DiseaseMedicalInstructionShare",
                columns: table => new
                {
                    DiseasesDiseaseId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MedicalInstructionSharesMedicalInstructionShareId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiseaseMedicalInstructionShare", x => new { x.DiseasesDiseaseId, x.MedicalInstructionSharesMedicalInstructionShareId });
                    table.ForeignKey(
                        name: "FK_DiseaseMedicalInstructionShare_Disease_DiseasesDiseaseId",
                        column: x => x.DiseasesDiseaseId,
                        principalTable: "Disease",
                        principalColumn: "DiseaseId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DiseaseMedicalInstructionShare_MedicalInstructionShare_MedicalInstructionSharesMedicalInstructionShareId",
                        column: x => x.MedicalInstructionSharesMedicalInstructionShareId,
                        principalTable: "MedicalInstructionShare",
                        principalColumn: "MedicalInstructionShareId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DiseaseMedicalInstructionShare_MedicalInstructionSharesMedicalInstructionShareId",
                table: "DiseaseMedicalInstructionShare",
                column: "MedicalInstructionSharesMedicalInstructionShareId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DiseaseMedicalInstructionShare");
        }
    }
}
