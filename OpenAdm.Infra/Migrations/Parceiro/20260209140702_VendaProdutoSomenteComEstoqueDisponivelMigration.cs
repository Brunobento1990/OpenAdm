using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OpenAdm.Infra.Migrations.Parceiro
{
    /// <inheritdoc />
    public partial class VendaProdutoSomenteComEstoqueDisponivelMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "VendaSomenteComEstoqueDisponivel",
                table: "Produtos",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VendaSomenteComEstoqueDisponivel",
                table: "Produtos");
        }
    }
}
