using Microsoft.EntityFrameworkCore.Migrations;

namespace HomeDoctor.Data.Migrations
{
    public partial class updateDB30 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Disease",
                newName: "Code");

            migrationBuilder.AddColumn<int>(
                name: "End",
                table: "Disease",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Number",
                table: "Disease",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Start",
                table: "Disease",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "End",
                table: "Disease");

            migrationBuilder.DropColumn(
                name: "Number",
                table: "Disease");

            migrationBuilder.DropColumn(
                name: "Start",
                table: "Disease");

            migrationBuilder.RenameColumn(
                name: "Code",
                table: "Disease",
                newName: "Status");
        }
    }
}
