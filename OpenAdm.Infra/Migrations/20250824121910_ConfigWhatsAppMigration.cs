using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OpenAdm.Infra.Migrations
{
    /// <inheritdoc />
    public partial class ConfigWhatsAppMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "WhatsApp",
                table: "ConfiguracoesDePedidos",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WhatsApp",
                table: "ConfiguracoesDePedidos");
        }
    }
}
