using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BedTrack.Migrations
{
    /// <inheritdoc />
    public partial class removed_bed_patient : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ClinicDepartmentBeds_PatientId",
                table: "ClinicDepartmentBeds");

            migrationBuilder.DropColumn(
                name: "ClinicDepartmentBedId",
                table: "Patients");

            migrationBuilder.CreateIndex(
                name: "IX_ClinicDepartmentBeds_PatientId",
                table: "ClinicDepartmentBeds",
                column: "PatientId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ClinicDepartmentBeds_PatientId",
                table: "ClinicDepartmentBeds");

            migrationBuilder.AddColumn<int>(
                name: "ClinicDepartmentBedId",
                table: "Patients",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ClinicDepartmentBeds_PatientId",
                table: "ClinicDepartmentBeds",
                column: "PatientId",
                unique: true,
                filter: "[PatientId] IS NOT NULL");
        }
    }
}
