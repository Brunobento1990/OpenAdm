using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OpenAdm.Infra.Migrations.Parceiro
{
    /// <inheritdoc />
    public partial class HotFIxMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "TabelaDePreco",
                columns: new[] { "Id", "AtivaEcommerce", "Descricao", "Numero" },
                values: new object[] { new Guid("4b43157b-8029-4e55-be53-1b8d27ea9be2"), true, "E-commerce", 1L });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "TabelaDePreco",
                keyColumn: "Id",
                keyValue: new Guid("4b43157b-8029-4e55-be53-1b8d27ea9be2"));
        }
    }
}
