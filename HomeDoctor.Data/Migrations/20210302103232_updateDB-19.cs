using Microsoft.EntityFrameworkCore.Migrations;

namespace HomeDoctor.Data.Migrations
{
    public partial class updateDB19 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Doctor_Role_RoleId",
                table: "Doctor");

            migrationBuilder.DropForeignKey(
                name: "FK_Patient_Role_RoleId",
                table: "Patient");

            migrationBuilder.DropIndex(
                name: "IX_Patient_RoleId",
                table: "Patient");

            migrationBuilder.DropIndex(
                name: "IX_Doctor_RoleId",
                table: "Doctor");

            migrationBuilder.DropColumn(
                name: "RoleId",
                table: "Patient");

            migrationBuilder.DropColumn(
                name: "RoleId",
                table: "Doctor");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RoleId",
                table: "Patient",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RoleId",
                table: "Doctor",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Patient_RoleId",
                table: "Patient",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Doctor_RoleId",
                table: "Doctor",
                column: "RoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Doctor_Role_RoleId",
                table: "Doctor",
                column: "RoleId",
                principalTable: "Role",
                principalColumn: "RoleId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Patient_Role_RoleId",
                table: "Patient",
                column: "RoleId",
                principalTable: "Role",
                principalColumn: "RoleId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
