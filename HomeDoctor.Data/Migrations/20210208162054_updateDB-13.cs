using Microsoft.EntityFrameworkCore.Migrations;

namespace HomeDoctor.Data.Migrations
{
    public partial class updateDB13 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PersonalHealthRecord_Patient_PersonalHealthRecordId",
                table: "PersonalHealthRecord");
            
            migrationBuilder.AlterColumn<int>(
                name: "PersonalHealthRecordId",
                table: "PersonalHealthRecord",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1");
            

            migrationBuilder.AddColumn<int>(
                name: "PatientId",
                table: "PersonalHealthRecord",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_PersonalHealthRecord_PatientId",
                table: "PersonalHealthRecord",
                column: "PatientId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PersonalHealthRecord_Patient_PatientId",
                table: "PersonalHealthRecord",
                column: "PatientId",
                principalTable: "Patient",
                principalColumn: "PatientId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PersonalHealthRecord_Patient_PatientId",
                table: "PersonalHealthRecord");

            migrationBuilder.DropIndex(
                name: "IX_PersonalHealthRecord_PatientId",
                table: "PersonalHealthRecord");

            migrationBuilder.DropColumn(
                name: "PatientId",
                table: "PersonalHealthRecord");

            migrationBuilder.AlterColumn<int>(
                name: "PersonalHealthRecordId",
                table: "PersonalHealthRecord",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddForeignKey(
                name: "FK_PersonalHealthRecord_Patient_PersonalHealthRecordId",
                table: "PersonalHealthRecord",
                column: "PersonalHealthRecordId",
                principalTable: "Patient",
                principalColumn: "PatientId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
