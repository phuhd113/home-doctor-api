using Microsoft.EntityFrameworkCore.Migrations;

namespace HomeDoctor.Data.Migrations
{
    public partial class updateDB39 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PersonalStatus",
                table: "PersonalHealthRecord",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AppointmentId",
                table: "Notification",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Notification_AppointmentId",
                table: "Notification",
                column: "AppointmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notification_Appointment_AppointmentId",
                table: "Notification",
                column: "AppointmentId",
                principalTable: "Appointment",
                principalColumn: "AppointmentId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notification_Appointment_AppointmentId",
                table: "Notification");

            migrationBuilder.DropIndex(
                name: "IX_Notification_AppointmentId",
                table: "Notification");

            migrationBuilder.DropColumn(
                name: "PersonalStatus",
                table: "PersonalHealthRecord");

            migrationBuilder.DropColumn(
                name: "AppointmentId",
                table: "Notification");
        }
    }
}
