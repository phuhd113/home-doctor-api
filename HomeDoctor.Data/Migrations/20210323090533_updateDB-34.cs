using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HomeDoctor.Data.Migrations
{
    public partial class updateDB34 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ActionFirstTime",
                columns: table => new
                {
                    ActionFirstTimeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PrescriptionFirst = table.Column<bool>(type: "bit", nullable: false),
                    AppointmentFirst = table.Column<bool>(type: "bit", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ContractId = table.Column<int>(type: "int", nullable: false)
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

            migrationBuilder.CreateTable(
                name: "ActionEveryWeek",
                columns: table => new
                {
                    ActionEveryWeekId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AppointmentWeek = table.Column<bool>(type: "bit", nullable: false),
                    PrescriptionWeek = table.Column<bool>(type: "bit", nullable: false),
                    VitalSignWeek = table.Column<bool>(type: "bit", nullable: false),
                    ActionFirstTimeId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActionEveryWeek", x => x.ActionEveryWeekId);
                    table.ForeignKey(
                        name: "FK_ActionEveryWeek_ActionFirstTime_ActionFirstTimeId",
                        column: x => x.ActionFirstTimeId,
                        principalTable: "ActionFirstTime",
                        principalColumn: "ActionFirstTimeId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ActionEveryDay",
                columns: table => new
                {
                    ActionEveryDayId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Examination = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ActionEveryWeekId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActionEveryDay", x => x.ActionEveryDayId);
                    table.ForeignKey(
                        name: "FK_ActionEveryDay_ActionEveryWeek_ActionEveryWeekId",
                        column: x => x.ActionEveryWeekId,
                        principalTable: "ActionEveryWeek",
                        principalColumn: "ActionEveryWeekId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActionEveryDay_ActionEveryWeekId",
                table: "ActionEveryDay",
                column: "ActionEveryWeekId");

            migrationBuilder.CreateIndex(
                name: "IX_ActionEveryWeek_ActionFirstTimeId",
                table: "ActionEveryWeek",
                column: "ActionFirstTimeId");

            migrationBuilder.CreateIndex(
                name: "IX_ActionFirstTime_ContractId",
                table: "ActionFirstTime",
                column: "ContractId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActionEveryDay");

            migrationBuilder.DropTable(
                name: "ActionEveryWeek");

            migrationBuilder.DropTable(
                name: "ActionFirstTime");
        }
    }
}
