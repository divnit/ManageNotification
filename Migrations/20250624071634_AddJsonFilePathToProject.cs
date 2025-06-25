using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ManageNotification.Migrations
{
    /// <inheritdoc />
    public partial class AddJsonFilePathToProject : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "JsonFilePath",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "JsonFilePath",
                table: "Projects");
        }
    }
}
