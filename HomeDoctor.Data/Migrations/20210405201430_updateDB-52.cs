using Microsoft.EntityFrameworkCore.Migrations;

namespace HomeDoctor.Data.Migrations
{
    public partial class updateDB52 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notification_Appointment_AppointmentId",
                table: "Notification");

            migrationBuilder.DropForeignKey(
                name: "FK_Notification_Contract_ContractId",
                table: "Notification");

            migrationBuilder.DropForeignKey(
                name: "FK_Notification_MedicalInstruction_MedicalInstructionId",
                table: "Notification");

            migrationBuilder.DropIndex(
                name: "IX_Notification_AppointmentId",
                table: "Notification");

            migrationBuilder.DropIndex(
                name: "IX_Notification_ContractId",
                table: "Notification");

            migrationBuilder.DropIndex(
                name: "IX_Notification_MedicalInstructionId",
                table: "Notification");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Notification_AppointmentId",
                table: "Notification",
                column: "AppointmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Notification_ContractId",
                table: "Notification",
                column: "ContractId");

            migrationBuilder.CreateIndex(
                name: "IX_Notification_MedicalInstructionId",
                table: "Notification",
                column: "MedicalInstructionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notification_Appointment_AppointmentId",
                table: "Notification",
                column: "AppointmentId",
                principalTable: "Appointment",
                principalColumn: "AppointmentId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Notification_Contract_ContractId",
                table: "Notification",
                column: "ContractId",
                principalTable: "Contract",
                principalColumn: "ContractId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Notification_MedicalInstruction_MedicalInstructionId",
                table: "Notification",
                column: "MedicalInstructionId",
                principalTable: "MedicalInstruction",
                principalColumn: "MedicalInstructionId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
