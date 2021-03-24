using Microsoft.EntityFrameworkCore.Migrations;

namespace HomeDoctor.Data.Migrations
{
    public partial class updateDB37 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MinuteAgain",
                table: "VitalSignSchedule",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MinuteDangerInterval",
                table: "VitalSignSchedule",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NumberMax",
                table: "VitalSignSchedule",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NumberMin",
                table: "VitalSignSchedule",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TimeStart",
                table: "VitalSignSchedule",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "VitalSignTypeId",
                table: "VitalSignSchedule",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "VitalSignType",
                columns: table => new
                {
                    VitalSignTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VitalSignName = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VitalSignType", x => x.VitalSignTypeId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VitalSignSchedule_VitalSignTypeId",
                table: "VitalSignSchedule",
                column: "VitalSignTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_VitalSignSchedule_VitalSignType_VitalSignTypeId",
                table: "VitalSignSchedule",
                column: "VitalSignTypeId",
                principalTable: "VitalSignType",
                principalColumn: "VitalSignTypeId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VitalSignSchedule_VitalSignType_VitalSignTypeId",
                table: "VitalSignSchedule");

            migrationBuilder.DropTable(
                name: "VitalSignType");

            migrationBuilder.DropIndex(
                name: "IX_VitalSignSchedule_VitalSignTypeId",
                table: "VitalSignSchedule");

            migrationBuilder.DropColumn(
                name: "MinuteAgain",
                table: "VitalSignSchedule");

            migrationBuilder.DropColumn(
                name: "MinuteDangerInterval",
                table: "VitalSignSchedule");

            migrationBuilder.DropColumn(
                name: "NumberMax",
                table: "VitalSignSchedule");

            migrationBuilder.DropColumn(
                name: "NumberMin",
                table: "VitalSignSchedule");

            migrationBuilder.DropColumn(
                name: "TimeStart",
                table: "VitalSignSchedule");

            migrationBuilder.DropColumn(
                name: "VitalSignTypeId",
                table: "VitalSignSchedule");
        }
    }
}
