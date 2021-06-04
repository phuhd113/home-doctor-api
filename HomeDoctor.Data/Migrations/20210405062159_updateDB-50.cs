using Microsoft.EntityFrameworkCore.Migrations;

namespace HomeDoctor.Data.Migrations
{
    public partial class updateDB50 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VitalSignValue_VitalSign_VitalSignId",
                table: "VitalSignValue");

            migrationBuilder.RenameColumn(
                name: "VitalSignId",
                table: "VitalSignValue",
                newName: "PatientId");

            migrationBuilder.RenameColumn(
                name: "TimeValue",
                table: "VitalSignValue",
                newName: "HeartBeatTimeValue");

            migrationBuilder.RenameColumn(
                name: "NumberValue",
                table: "VitalSignValue",
                newName: "HeartBeatNumberValue");

            migrationBuilder.RenameIndex(
                name: "IX_VitalSignValue_VitalSignId",
                table: "VitalSignValue",
                newName: "IX_VitalSignValue_PatientId");

            migrationBuilder.AddColumn<string>(
                name: "BloodPressureNumberValue",
                table: "VitalSignValue",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BloodPressureTimeValue",
                table: "VitalSignValue",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_VitalSignValue_Patient_PatientId",
                table: "VitalSignValue",
                column: "PatientId",
                principalTable: "Patient",
                principalColumn: "PatientId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VitalSignValue_Patient_PatientId",
                table: "VitalSignValue");

            migrationBuilder.DropColumn(
                name: "BloodPressureNumberValue",
                table: "VitalSignValue");

            migrationBuilder.DropColumn(
                name: "BloodPressureTimeValue",
                table: "VitalSignValue");

            migrationBuilder.RenameColumn(
                name: "PatientId",
                table: "VitalSignValue",
                newName: "VitalSignId");

            migrationBuilder.RenameColumn(
                name: "HeartBeatTimeValue",
                table: "VitalSignValue",
                newName: "TimeValue");

            migrationBuilder.RenameColumn(
                name: "HeartBeatNumberValue",
                table: "VitalSignValue",
                newName: "NumberValue");

            migrationBuilder.RenameIndex(
                name: "IX_VitalSignValue_PatientId",
                table: "VitalSignValue",
                newName: "IX_VitalSignValue_VitalSignId");

            migrationBuilder.AddForeignKey(
                name: "FK_VitalSignValue_VitalSign_VitalSignId",
                table: "VitalSignValue",
                column: "VitalSignId",
                principalTable: "VitalSign",
                principalColumn: "VitalSignId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
