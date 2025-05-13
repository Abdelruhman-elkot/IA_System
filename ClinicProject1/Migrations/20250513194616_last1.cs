using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClinicProject1.Migrations
{
    /// <inheritdoc />
    public partial class last1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "targetUser",
                table: "chatsHistories",
                newName: "targetUserId");

            migrationBuilder.RenameColumn(
                name: "senderUsername",
                table: "chatsHistories",
                newName: "senderUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "targetUserId",
                table: "chatsHistories",
                newName: "targetUser");

            migrationBuilder.RenameColumn(
                name: "senderUserId",
                table: "chatsHistories",
                newName: "senderUsername");
        }
    }
}
