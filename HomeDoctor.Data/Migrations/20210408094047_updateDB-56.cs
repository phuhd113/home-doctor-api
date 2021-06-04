using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HomeDoctor.Data.Migrations
{
    public partial class updateDB56 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointment_Contract_ContractId",
                table: "Appointment");

            migrationBuilder.DropTable(
                name: "FireBaseFCM");

            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "Appointment");

            migrationBuilder.AlterColumn<int>(
                name: "ContractId",
                table: "Appointment",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "MedicalInstructionId",
                table: "Appointment",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "FireBaseToken",
                table: "Account",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Appointment_MedicalInstructionId",
                table: "Appointment",
                column: "MedicalInstructionId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Appointment_Contract_ContractId",
                table: "Appointment",
                column: "ContractId",
                principalTable: "Contract",
                principalColumn: "ContractId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Appointment_MedicalInstruction_MedicalInstructionId",
                table: "Appointment",
                column: "MedicalInstructionId",
                principalTable: "MedicalInstruction",
                principalColumn: "MedicalInstructionId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointment_Contract_ContractId",
                table: "Appointment");

            migrationBuilder.DropForeignKey(
                name: "FK_Appointment_MedicalInstruction_MedicalInstructionId",
                table: "Appointment");

            migrationBuilder.DropIndex(
                name: "IX_Appointment_MedicalInstructionId",
                table: "Appointment");

            migrationBuilder.DropColumn(
                name: "MedicalInstructionId",
                table: "Appointment");

            migrationBuilder.DropColumn(
                name: "FireBaseToken",
                table: "Account");

            migrationBuilder.AlterColumn<int>(
                name: "ContractId",
                table: "Appointment",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "Appointment",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "FireBaseFCM",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountId = table.Column<int>(type: "int", nullable: false),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FireBaseFCM", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FireBaseFCM_Account_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FireBaseFCM_AccountId",
                table: "FireBaseFCM",
                column: "AccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointment_Contract_ContractId",
                table: "Appointment",
                column: "ContractId",
                principalTable: "Contract",
                principalColumn: "ContractId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
