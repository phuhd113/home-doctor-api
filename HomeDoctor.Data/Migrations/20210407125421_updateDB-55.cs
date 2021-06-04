using Microsoft.EntityFrameworkCore.Migrations;

namespace HomeDoctor.Data.Migrations
{
    public partial class updateDB55 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MedicalInstructionImage_MedicalInstruction_MedicalInstructionId",
                table: "MedicalInstructionImage");

            migrationBuilder.DropIndex(
                name: "IX_MedicalInstructionImage_MedicalInstructionId",
                table: "MedicalInstructionImage");

            migrationBuilder.DropColumn(
                name: "MedicalInstructionId",
                table: "MedicalInstructionImage");

            migrationBuilder.CreateTable(
                name: "MedicalInstructionMedicalInstructionImage",
                columns: table => new
                {
                    MedicalInstructionImagesMedicalInstructionImageId = table.Column<int>(type: "int", nullable: false),
                    MedicalInstructionsMedicalInstructionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicalInstructionMedicalInstructionImage", x => new { x.MedicalInstructionImagesMedicalInstructionImageId, x.MedicalInstructionsMedicalInstructionId });
                    table.ForeignKey(
                        name: "FK_MedicalInstructionMedicalInstructionImage_MedicalInstruction_MedicalInstructionsMedicalInstructionId",
                        column: x => x.MedicalInstructionsMedicalInstructionId,
                        principalTable: "MedicalInstruction",
                        principalColumn: "MedicalInstructionId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MedicalInstructionMedicalInstructionImage_MedicalInstructionImage_MedicalInstructionImagesMedicalInstructionImageId",
                        column: x => x.MedicalInstructionImagesMedicalInstructionImageId,
                        principalTable: "MedicalInstructionImage",
                        principalColumn: "MedicalInstructionImageId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MedicalInstructionMedicalInstructionImage_MedicalInstructionsMedicalInstructionId",
                table: "MedicalInstructionMedicalInstructionImage",
                column: "MedicalInstructionsMedicalInstructionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MedicalInstructionMedicalInstructionImage");

            migrationBuilder.AddColumn<int>(
                name: "MedicalInstructionId",
                table: "MedicalInstructionImage",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_MedicalInstructionImage_MedicalInstructionId",
                table: "MedicalInstructionImage",
                column: "MedicalInstructionId");

            migrationBuilder.AddForeignKey(
                name: "FK_MedicalInstructionImage_MedicalInstruction_MedicalInstructionId",
                table: "MedicalInstructionImage",
                column: "MedicalInstructionId",
                principalTable: "MedicalInstruction",
                principalColumn: "MedicalInstructionId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
