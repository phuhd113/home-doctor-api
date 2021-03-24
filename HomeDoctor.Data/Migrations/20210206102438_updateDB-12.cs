using Microsoft.EntityFrameworkCore.Migrations;

namespace HomeDoctor.Data.Migrations
{
    public partial class updateDB12 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ContractId",
                table: "HealthRecord",
                type: "int",
                nullable: false,
                defaultValue: 0);

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HealthRecord_Contract_ContractId",
                table: "HealthRecord");

            migrationBuilder.DropIndex(
                name: "IX_HealthRecord_ContractId",
                table: "HealthRecord");

            migrationBuilder.DropColumn(
                name: "ContractId",
                table: "HealthRecord");
        }
    }
}
