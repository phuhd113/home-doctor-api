using Microsoft.EntityFrameworkCore.Migrations;

namespace HomeDoctor.Data.Migrations
{
    public partial class updateDB24 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Dicease",
                table: "HealthRecord");

            migrationBuilder.CreateTable(
                name: "DiseaseHealthRecord",
                columns: table => new
                {
                    DiseasesDiseaseId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    HealthRecordsHealthRecordId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiseaseHealthRecord", x => new { x.DiseasesDiseaseId, x.HealthRecordsHealthRecordId });
                    table.ForeignKey(
                        name: "FK_DiseaseHealthRecord_Disease_DiseasesDiseaseId",
                        column: x => x.DiseasesDiseaseId,
                        principalTable: "Disease",
                        principalColumn: "DiseaseId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DiseaseHealthRecord_HealthRecord_HealthRecordsHealthRecordId",
                        column: x => x.HealthRecordsHealthRecordId,
                        principalTable: "HealthRecord",
                        principalColumn: "HealthRecordId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DiseaseHealthRecord_HealthRecordsHealthRecordId",
                table: "DiseaseHealthRecord",
                column: "HealthRecordsHealthRecordId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DiseaseHealthRecord");

            migrationBuilder.AddColumn<string>(
                name: "Dicease",
                table: "HealthRecord",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
