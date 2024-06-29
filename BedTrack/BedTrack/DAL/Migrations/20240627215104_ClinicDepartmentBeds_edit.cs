using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BedTrack.Migrations
{
    /// <inheritdoc />
    public partial class ClinicDepartmentBeds_edit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Patients_ClinicDepartmentBeds_ClinicDepartmentBedId",
                table: "Patients");

            migrationBuilder.DropIndex(
                name: "IX_Patients_ClinicDepartmentBedId",
                table: "Patients");

            migrationBuilder.AddColumn<int>(
                name: "PatientId",
                table: "ClinicDepartmentBeds",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClinicDepartmentBeds_PatientId",
                table: "ClinicDepartmentBeds",
                column: "PatientId",
                unique: true,
                filter: "[PatientId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_ClinicDepartmentBeds_Patients_PatientId",
                table: "ClinicDepartmentBeds",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClinicDepartmentBeds_Patients_PatientId",
                table: "ClinicDepartmentBeds");

            migrationBuilder.DropIndex(
                name: "IX_ClinicDepartmentBeds_PatientId",
                table: "ClinicDepartmentBeds");

            migrationBuilder.DropColumn(
                name: "PatientId",
                table: "ClinicDepartmentBeds");

            migrationBuilder.CreateIndex(
                name: "IX_Patients_ClinicDepartmentBedId",
                table: "Patients",
                column: "ClinicDepartmentBedId");

            migrationBuilder.AddForeignKey(
                name: "FK_Patients_ClinicDepartmentBeds_ClinicDepartmentBedId",
                table: "Patients",
                column: "ClinicDepartmentBedId",
                principalTable: "ClinicDepartmentBeds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
