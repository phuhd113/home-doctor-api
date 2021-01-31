using Microsoft.EntityFrameworkCore.Migrations;

namespace HomeDoctor.Data.Migrations
{
    public partial class updateDB10 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Disease_Contract_ContractId",
                table: "Disease");

            migrationBuilder.DropIndex(
                name: "IX_Disease_ContractId",
                table: "Disease");

            migrationBuilder.DropColumn(
                name: "ContractId",
                table: "Disease");

            migrationBuilder.AddColumn<int>(
                name: "LicenseId",
                table: "Contract",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ContractDisease",
                columns: table => new
                {
                    ContractsContractId = table.Column<int>(type: "int", nullable: false),
                    DiseasesDiseaseId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContractDisease", x => new { x.ContractsContractId, x.DiseasesDiseaseId });
                    table.ForeignKey(
                        name: "FK_ContractDisease_Contract_ContractsContractId",
                        column: x => x.ContractsContractId,
                        principalTable: "Contract",
                        principalColumn: "ContractId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ContractDisease_Disease_DiseasesDiseaseId",
                        column: x => x.DiseasesDiseaseId,
                        principalTable: "Disease",
                        principalColumn: "DiseaseId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Contract_LicenseId",
                table: "Contract",
                column: "LicenseId");

            migrationBuilder.CreateIndex(
                name: "IX_ContractDisease_DiseasesDiseaseId",
                table: "ContractDisease",
                column: "DiseasesDiseaseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Contract_License_LicenseId",
                table: "Contract",
                column: "LicenseId",
                principalTable: "License",
                principalColumn: "LicenseId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contract_License_LicenseId",
                table: "Contract");

            migrationBuilder.DropTable(
                name: "ContractDisease");

            migrationBuilder.DropIndex(
                name: "IX_Contract_LicenseId",
                table: "Contract");

            migrationBuilder.DropColumn(
                name: "LicenseId",
                table: "Contract");

            migrationBuilder.AddColumn<int>(
                name: "ContractId",
                table: "Disease",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Disease_ContractId",
                table: "Disease",
                column: "ContractId");

            migrationBuilder.AddForeignKey(
                name: "FK_Disease_Contract_ContractId",
                table: "Disease",
                column: "ContractId",
                principalTable: "Contract",
                principalColumn: "ContractId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
