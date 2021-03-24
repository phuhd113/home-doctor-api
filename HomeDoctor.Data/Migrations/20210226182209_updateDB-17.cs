using Microsoft.EntityFrameworkCore.Migrations;

namespace HomeDoctor.Data.Migrations
{
    public partial class updateDB17 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HealthRecord_Contract_ContractId",
                table: "HealthRecord");

            migrationBuilder.DropIndex(
                name: "IX_HealthRecord_ContractId",
                table: "HealthRecord");

            migrationBuilder.AlterColumn<int>(
                name: "ContractId",
                table: "HealthRecord",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_HealthRecord_ContractId",
                table: "HealthRecord",
                column: "ContractId",
                unique: true,
                filter: "[ContractId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_HealthRecord_Contract_ContractId",
                table: "HealthRecord",
                column: "ContractId",
                principalTable: "Contract",
                principalColumn: "ContractId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HealthRecord_Contract_ContractId",
                table: "HealthRecord");

            migrationBuilder.DropIndex(
                name: "IX_HealthRecord_ContractId",
                table: "HealthRecord");

            migrationBuilder.AlterColumn<int>(
                name: "ContractId",
                table: "HealthRecord",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_HealthRecord_ContractId",
                table: "HealthRecord",
                column: "ContractId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_HealthRecord_Contract_ContractId",
                table: "HealthRecord",
                column: "ContractId",
                principalTable: "Contract",
                principalColumn: "ContractId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
