using Microsoft.EntityFrameworkCore.Migrations;

namespace HomeDoctor.Data.Migrations
{
    public partial class updateDB63 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VitalSignValue_Patient_PatientId",
                table: "VitalSignValue");

            migrationBuilder.RenameColumn(
                name: "PatientId",
                table: "VitalSignValue",
                newName: "PersonalHealthRecordId");

            migrationBuilder.RenameIndex(
                name: "IX_VitalSignValue_PatientId",
                table: "VitalSignValue",
                newName: "IX_VitalSignValue_PersonalHealthRecordId");

            migrationBuilder.AddForeignKey(
                name: "FK_VitalSignValue_PersonalHealthRecord_PersonalHealthRecordId",
                table: "VitalSignValue",
                column: "PersonalHealthRecordId",
                principalTable: "PersonalHealthRecord",
                principalColumn: "PersonalHealthRecordId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VitalSignValue_PersonalHealthRecord_PersonalHealthRecordId",
                table: "VitalSignValue");

            migrationBuilder.RenameColumn(
                name: "PersonalHealthRecordId",
                table: "VitalSignValue",
                newName: "PatientId");

            migrationBuilder.RenameIndex(
                name: "IX_VitalSignValue_PersonalHealthRecordId",
                table: "VitalSignValue",
                newName: "IX_VitalSignValue_PatientId");

            migrationBuilder.AddForeignKey(
                name: "FK_VitalSignValue_Patient_PatientId",
                table: "VitalSignValue",
                column: "PatientId",
                principalTable: "Patient",
                principalColumn: "PatientId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
