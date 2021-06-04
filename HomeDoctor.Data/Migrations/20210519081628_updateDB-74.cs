using Microsoft.EntityFrameworkCore.Migrations;

namespace HomeDoctor.Data.Migrations
{
    public partial class updateDB74 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DiseaseId",
                table: "ContractMedicalInstruction",
                newName: "DiseaseIds");

            migrationBuilder.AddColumn<string>(
                name: "DiseaseChoosedId",
                table: "ContractMedicalInstruction",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DiseaseChoosedId",
                table: "ContractMedicalInstruction");

            migrationBuilder.RenameColumn(
                name: "DiseaseIds",
                table: "ContractMedicalInstruction",
                newName: "DiseaseId");
        }
    }
}
