using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Proyecto_Tesis.Migrations
{
    /// <inheritdoc />
    public partial class AddInputSizeToLogs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "InputSize",
                table: "Logs",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InputSize",
                table: "Logs");
        }
    }
}
