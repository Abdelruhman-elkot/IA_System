using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClinicProject1.Migrations
{
    /// <inheritdoc />
    public partial class T9 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Doctors_Users_DoctorId",
                table: "Doctors");

            migrationBuilder.AddForeignKey(
                name: "FK_Doctors_Users_DoctorId",
                table: "Doctors",
                column: "DoctorId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Doctors_Users_DoctorId",
                table: "Doctors");

            migrationBuilder.AddForeignKey(
                name: "FK_Doctors_Users_DoctorId",
                table: "Doctors",
                column: "DoctorId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
