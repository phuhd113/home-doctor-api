using Microsoft.EntityFrameworkCore.Migrations;

namespace HomeDoctor.Data.Migrations
{
    public partial class updateDB18 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Account_Role_RoleId",
                table: "Account");

            migrationBuilder.DropIndex(
                name: "IX_Patient_AccountId",
                table: "Patient");

            migrationBuilder.DropIndex(
                name: "IX_Doctor_AccountId",
                table: "Doctor");

            migrationBuilder.DropColumn(
                name: "Password",
                table: "Doctor");

            migrationBuilder.DropColumn(
                name: "Username",
                table: "Doctor");

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

            migrationBuilder.AlterColumn<int>(
                name: "RoleId",
                table: "Account",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Account",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "Account",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Patient_AccountId",
                table: "Patient",
                column: "AccountId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Patient_RoleId",
                table: "Patient",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Doctor_AccountId",
                table: "Doctor",
                column: "AccountId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Doctor_RoleId",
                table: "Doctor",
                column: "RoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Account_Role_RoleId",
                table: "Account",
                column: "RoleId",
                principalTable: "Role",
                principalColumn: "RoleId",
                onDelete: ReferentialAction.Cascade);

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Account_Role_RoleId",
                table: "Account");

            migrationBuilder.DropForeignKey(
                name: "FK_Doctor_Role_RoleId",
                table: "Doctor");

            migrationBuilder.DropForeignKey(
                name: "FK_Patient_Role_RoleId",
                table: "Patient");

            migrationBuilder.DropIndex(
                name: "IX_Patient_AccountId",
                table: "Patient");

            migrationBuilder.DropIndex(
                name: "IX_Patient_RoleId",
                table: "Patient");

            migrationBuilder.DropIndex(
                name: "IX_Doctor_AccountId",
                table: "Doctor");

            migrationBuilder.DropIndex(
                name: "IX_Doctor_RoleId",
                table: "Doctor");

            migrationBuilder.DropColumn(
                name: "RoleId",
                table: "Patient");

            migrationBuilder.DropColumn(
                name: "RoleId",
                table: "Doctor");

            migrationBuilder.DropColumn(
                name: "Password",
                table: "Account");

            migrationBuilder.DropColumn(
                name: "Username",
                table: "Account");

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Doctor",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "Doctor",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "RoleId",
                table: "Account",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Patient_AccountId",
                table: "Patient",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Doctor_AccountId",
                table: "Doctor",
                column: "AccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Account_Role_RoleId",
                table: "Account",
                column: "RoleId",
                principalTable: "Role",
                principalColumn: "RoleId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
