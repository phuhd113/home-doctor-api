using Microsoft.EntityFrameworkCore.Migrations;

namespace HomeDoctor.Data.Migrations
{
    public partial class updateDBlast : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumberStartValue",
                table: "VitalSignValue");

            migrationBuilder.DropColumn(
                name: "TimeStartValue",
                table: "VitalSignValue");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NumberStartValue",
                table: "VitalSignValue",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TimeStartValue",
                table: "VitalSignValue",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
