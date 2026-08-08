using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OpenAdm.Data.Migrations
{
    /// <inheritdoc />
    public partial class AjustesFinanceiroMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DataDePagamento",
                table: "TransacoesFinanceiras",
                newName: "DataDeEfetivacao");

            migrationBuilder.AddColumn<decimal>(
                name: "Desconto",
                table: "TransacoesFinanceiras",
                type: "numeric(12,2)",
                precision: 12,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "FoiEstornado",
                table: "TransacoesFinanceiras",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Juros",
                table: "TransacoesFinanceiras",
                type: "numeric(12,2)",
                precision: 12,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Juros",
                table: "Parcelas",
                type: "numeric(12,2)",
                precision: 12,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Quitada",
                table: "Parcelas",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Tipo",
                table: "Parcelas",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Desconto",
                table: "TransacoesFinanceiras");

            migrationBuilder.DropColumn(
                name: "FoiEstornado",
                table: "TransacoesFinanceiras");

            migrationBuilder.DropColumn(
                name: "Juros",
                table: "TransacoesFinanceiras");

            migrationBuilder.DropColumn(
                name: "Juros",
                table: "Parcelas");

            migrationBuilder.DropColumn(
                name: "Quitada",
                table: "Parcelas");

            migrationBuilder.DropColumn(
                name: "Tipo",
                table: "Parcelas");

            migrationBuilder.RenameColumn(
                name: "DataDeEfetivacao",
                table: "TransacoesFinanceiras",
                newName: "DataDePagamento");
        }
    }
}
