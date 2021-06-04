using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HomeDoctor.Data.Migrations
{
    public partial class updateDB40 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "VitalSignName",
                table: "VitalSignType",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateTable(
                name: "BloodPressure",
                columns: table => new
                {
                    BloodPressureId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BloodPressureRate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TimeCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    VitalSignId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BloodPressure", x => x.BloodPressureId);
                    table.ForeignKey(
                        name: "FK_BloodPressure_VitalSign_VitalSignId",
                        column: x => x.VitalSignId,
                        principalTable: "VitalSign",
                        principalColumn: "VitalSignId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HeartBeat",
                columns: table => new
                {
                    HeartBeatId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HeartRate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VitalSignId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HeartBeat", x => x.HeartBeatId);
                    table.ForeignKey(
                        name: "FK_HeartBeat_VitalSign_VitalSignId",
                        column: x => x.VitalSignId,
                        principalTable: "VitalSign",
                        principalColumn: "VitalSignId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BloodPressure_VitalSignId",
                table: "BloodPressure",
                column: "VitalSignId");

            migrationBuilder.CreateIndex(
                name: "IX_HeartBeat_VitalSignId",
                table: "HeartBeat",
                column: "VitalSignId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BloodPressure");

            migrationBuilder.DropTable(
                name: "HeartBeat");

            migrationBuilder.AlterColumn<int>(
                name: "VitalSignName",
                table: "VitalSignType",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
