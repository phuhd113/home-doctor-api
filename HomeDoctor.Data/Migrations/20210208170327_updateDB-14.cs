using Microsoft.EntityFrameworkCore.Migrations;

namespace HomeDoctor.Data.Migrations
{
    public partial class updateDB14 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Amount",
                table: "MedicationSchedule",
                newName: "Number");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "MedicalInstruction",
                newName: "Description");

            migrationBuilder.AlterColumn<string>(
                name: "Unit",
                table: "MedicationSchedule",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Number",
                table: "MedicationSchedule",
                newName: "Amount");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "MedicalInstruction",
                newName: "Name");

            migrationBuilder.AlterColumn<int>(
                name: "Unit",
                table: "MedicationSchedule",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
