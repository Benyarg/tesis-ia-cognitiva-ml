using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Proyecto_Tesis.Migrations
{
    /// <inheritdoc />
    public partial class FixSUS : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "PuntajeTotal",
                table: "ResultadosSUS",
                type: "float",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "PuntajeTotal",
                table: "ResultadosSUS",
                type: "int",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");
        }
    }
}
