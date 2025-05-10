using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClinicProject1.Migrations
{
    /// <inheritdoc />
    public partial class ws : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "chatsHistories",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    targetUser = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    chatMessage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    senderUsername = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_chatsHistories", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "chatsHistories");
        }
    }
}
