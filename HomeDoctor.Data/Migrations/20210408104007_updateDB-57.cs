using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HomeDoctor.Data.Migrations
{
    public partial class updateDB57 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActionFirstTime");

            migrationBuilder.AddColumn<bool>(
                name: "AppointmentFirst",
                table: "HealthRecord",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "PrescriptionFirst",
                table: "HealthRecord",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AppointmentFirst",
                table: "HealthRecord");

            migrationBuilder.DropColumn(
                name: "PrescriptionFirst",
                table: "HealthRecord");

            migrationBuilder.CreateTable(
                name: "ActionFirstTime",
                columns: table => new
                {
                    ActionFirstTimeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppointmentFirst = table.Column<bool>(type: "bit", nullable: false),
                    ContractId = table.Column<int>(type: "int", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PrescriptionFirst = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActionFirstTime", x => x.ActionFirstTimeId);
                    table.ForeignKey(
                        name: "FK_ActionFirstTime_Contract_ContractId",
                        column: x => x.ContractId,
                        principalTable: "Contract",
                        principalColumn: "ContractId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActionFirstTime_ContractId",
                table: "ActionFirstTime",
                column: "ContractId",
                unique: true);
        }
    }
}
