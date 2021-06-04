using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HomeDoctor.Data.Migrations
{
    public partial class updateDB72 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContractMedicalInstructions_Contract_ContractId",
                table: "ContractMedicalInstructions");

            migrationBuilder.DropForeignKey(
                name: "FK_ContractMedicalInstructions_MedicalInstruction_MedicalInstructionId",
                table: "ContractMedicalInstructions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ContractMedicalInstructions",
                table: "ContractMedicalInstructions");

            migrationBuilder.RenameTable(
                name: "ContractMedicalInstructions",
                newName: "ContractMedicalInstruction");

            migrationBuilder.RenameIndex(
                name: "IX_ContractMedicalInstructions_MedicalInstructionId",
                table: "ContractMedicalInstruction",
                newName: "IX_ContractMedicalInstruction_MedicalInstructionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ContractMedicalInstruction",
                table: "ContractMedicalInstruction",
                columns: new[] { "ContractId", "MedicalInstructionId" });

            migrationBuilder.CreateTable(
                name: "VitalSignValueShare",
                columns: table => new
                {
                    VitalSignValueShareId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HealthRecordId = table.Column<int>(type: "int", nullable: false),
                    TimeShare = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MinuteShare = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VitalSignValueShare", x => x.VitalSignValueShareId);
                    table.ForeignKey(
                        name: "FK_VitalSignValueShare_HealthRecord_HealthRecordId",
                        column: x => x.HealthRecordId,
                        principalTable: "HealthRecord",
                        principalColumn: "HealthRecordId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VitalSignValueShare_HealthRecordId",
                table: "VitalSignValueShare",
                column: "HealthRecordId");

            migrationBuilder.AddForeignKey(
                name: "FK_ContractMedicalInstruction_Contract_ContractId",
                table: "ContractMedicalInstruction",
                column: "ContractId",
                principalTable: "Contract",
                principalColumn: "ContractId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ContractMedicalInstruction_MedicalInstruction_MedicalInstructionId",
                table: "ContractMedicalInstruction",
                column: "MedicalInstructionId",
                principalTable: "MedicalInstruction",
                principalColumn: "MedicalInstructionId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContractMedicalInstruction_Contract_ContractId",
                table: "ContractMedicalInstruction");

            migrationBuilder.DropForeignKey(
                name: "FK_ContractMedicalInstruction_MedicalInstruction_MedicalInstructionId",
                table: "ContractMedicalInstruction");

            migrationBuilder.DropTable(
                name: "VitalSignValueShare");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ContractMedicalInstruction",
                table: "ContractMedicalInstruction");

            migrationBuilder.RenameTable(
                name: "ContractMedicalInstruction",
                newName: "ContractMedicalInstructions");

            migrationBuilder.RenameIndex(
                name: "IX_ContractMedicalInstruction_MedicalInstructionId",
                table: "ContractMedicalInstructions",
                newName: "IX_ContractMedicalInstructions_MedicalInstructionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ContractMedicalInstructions",
                table: "ContractMedicalInstructions",
                columns: new[] { "ContractId", "MedicalInstructionId" });

            migrationBuilder.AddForeignKey(
                name: "FK_ContractMedicalInstructions_Contract_ContractId",
                table: "ContractMedicalInstructions",
                column: "ContractId",
                principalTable: "Contract",
                principalColumn: "ContractId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ContractMedicalInstructions_MedicalInstruction_MedicalInstructionId",
                table: "ContractMedicalInstructions",
                column: "MedicalInstructionId",
                principalTable: "MedicalInstruction",
                principalColumn: "MedicalInstructionId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
