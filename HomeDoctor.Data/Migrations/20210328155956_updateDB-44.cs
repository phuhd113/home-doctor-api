using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HomeDoctor.Data.Migrations
{
    public partial class updateDB44 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HeartBeat");

            migrationBuilder.CreateTable(
                name: "VitalSignValue",
                columns: table => new
                {
                    VitalSignValueId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TimeValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NumberValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VitalSignId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VitalSignValue", x => x.VitalSignValueId);
                    table.ForeignKey(
                        name: "FK_VitalSignValue_VitalSign_VitalSignId",
                        column: x => x.VitalSignId,
                        principalTable: "VitalSign",
                        principalColumn: "VitalSignId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VitalSignValue_VitalSignId",
                table: "VitalSignValue",
                column: "VitalSignId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VitalSignValue");

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
                name: "IX_HeartBeat_VitalSignId",
                table: "HeartBeat",
                column: "VitalSignId");
        }
    }
}
