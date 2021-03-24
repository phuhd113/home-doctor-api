using Microsoft.EntityFrameworkCore.Migrations;

namespace HomeDoctor.Data.Migrations
{
    public partial class updateDB21 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AccountId",
                table: "FireBaseFCM",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_FireBaseFCM_AccountId",
                table: "FireBaseFCM",
                column: "AccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_FireBaseFCM_Account_AccountId",
                table: "FireBaseFCM",
                column: "AccountId",
                principalTable: "Account",
                principalColumn: "AccountId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FireBaseFCM_Account_AccountId",
                table: "FireBaseFCM");

            migrationBuilder.DropIndex(
                name: "IX_FireBaseFCM_AccountId",
                table: "FireBaseFCM");

            migrationBuilder.DropColumn(
                name: "AccountId",
                table: "FireBaseFCM");
        }
    }
}
