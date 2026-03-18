using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OpenAdm.Infra.Migrations.Parceiro
{
    /// <inheritdoc />
    public partial class ColunasParaFreteMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "PesoReal",
                table: "Tamanhos",
                type: "numeric(12,2)",
                precision: 12,
                scale: 2,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric",
                oldNullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "AlturaReal",
                table: "Tamanhos",
                type: "numeric(12,2)",
                precision: 12,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ComprimentoReal",
                table: "Tamanhos",
                type: "numeric(12,2)",
                precision: 12,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "LarguraReal",
                table: "Tamanhos",
                type: "numeric(12,2)",
                precision: 12,
                scale: 2,
                nullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "PesoReal",
                table: "Pesos",
                type: "numeric(12,2)",
                precision: 12,
                scale: 2,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric",
                oldNullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "AlturaReal",
                table: "Pesos",
                type: "numeric(12,2)",
                precision: 12,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ComprimentoReal",
                table: "Pesos",
                type: "numeric(12,2)",
                precision: 12,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "LarguraReal",
                table: "Pesos",
                type: "numeric(12,2)",
                precision: 12,
                scale: 2,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AlturaReal",
                table: "Tamanhos");

            migrationBuilder.DropColumn(
                name: "ComprimentoReal",
                table: "Tamanhos");

            migrationBuilder.DropColumn(
                name: "LarguraReal",
                table: "Tamanhos");

            migrationBuilder.DropColumn(
                name: "AlturaReal",
                table: "Pesos");

            migrationBuilder.DropColumn(
                name: "ComprimentoReal",
                table: "Pesos");

            migrationBuilder.DropColumn(
                name: "LarguraReal",
                table: "Pesos");

            migrationBuilder.AlterColumn<decimal>(
                name: "PesoReal",
                table: "Tamanhos",
                type: "numeric",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(12,2)",
                oldPrecision: 12,
                oldScale: 2,
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "PesoReal",
                table: "Pesos",
                type: "numeric",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(12,2)",
                oldPrecision: 12,
                oldScale: 2,
                oldNullable: true);
        }
    }
}
