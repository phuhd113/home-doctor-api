using Microsoft.EntityFrameworkCore.Migrations;

namespace HomeDoctor.Data.Migrations
{
    public partial class updateDB49 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Relative_Account_AccountId",
                table: "Relative");

            migrationBuilder.DropIndex(
                name: "IX_Relative_AccountId",
                table: "Relative");

            migrationBuilder.DropColumn(
                name: "AccountId",
                table: "Relative");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AccountId",
                table: "Relative",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Relative_AccountId",
                table: "Relative",
                column: "AccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Relative_Account_AccountId",
                table: "Relative",
                column: "AccountId",
                principalTable: "Account",
                principalColumn: "AccountId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
