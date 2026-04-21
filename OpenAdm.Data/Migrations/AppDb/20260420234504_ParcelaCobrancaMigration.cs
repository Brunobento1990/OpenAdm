using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OpenAdm.Data.Migrations.AppDb
{
    /// <inheritdoc />
    public partial class ParcelaCobrancaMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TipoParcelaCobranca",
                table: "Empresas",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ParcelasCobrancas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DataDeCadastro = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    DataDeVencimento = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    DataDePagamento = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Numero = table.Column<int>(type: "integer", nullable: false),
                    MesCobranca = table.Column<int>(type: "integer", nullable: false),
                    AnoCobranca = table.Column<int>(type: "integer", nullable: false),
                    Valor = table.Column<decimal>(type: "numeric(12,2)", precision: 12, scale: 2, nullable: false),
                    ValorPago = table.Column<decimal>(type: "numeric(12,2)", precision: 12, scale: 2, nullable: false),
                    TipoParcelaCobranca = table.Column<int>(type: "integer", nullable: false),
                    EmpresaOpenAdmId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParcelasCobrancas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ParcelasCobrancas_Empresas_EmpresaOpenAdmId",
                        column: x => x.EmpresaOpenAdmId,
                        principalTable: "Empresas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Empresas_TipoParcelaCobranca",
                table: "Empresas",
                column: "TipoParcelaCobranca");

            migrationBuilder.CreateIndex(
                name: "IX_ParcelasCobrancas_EmpresaOpenAdmId_AnoCobranca_MesCobranca",
                table: "ParcelasCobrancas",
                columns: new[] { "EmpresaOpenAdmId", "AnoCobranca", "MesCobranca" });

            migrationBuilder.CreateIndex(
                name: "IX_ParcelasCobrancas_EmpresaOpenAdmId_AnoCobranca_MesCobranca_~",
                table: "ParcelasCobrancas",
                columns: new[] { "EmpresaOpenAdmId", "AnoCobranca", "MesCobranca", "Numero" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ParcelasCobrancas");

            migrationBuilder.DropIndex(
                name: "IX_Empresas_TipoParcelaCobranca",
                table: "Empresas");

            migrationBuilder.DropColumn(
                name: "TipoParcelaCobranca",
                table: "Empresas");
        }
    }
}
