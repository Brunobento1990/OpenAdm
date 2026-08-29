using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using OpenAdm.Data.Context;

#nullable disable

namespace OpenAdm.Data.Migrations;

[DbContext(typeof(ParceiroContext))]
[Migration("20260829120000_PedidoIdPublicoMigration")]
public partial class PedidoIdPublicoMigration : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<Guid>(
            name: "IdPublico",
            table: "Pedidos",
            type: "uuid",
            nullable: false,
            defaultValueSql: "gen_random_uuid()");

        migrationBuilder.CreateIndex(
            name: "IX_Pedidos_IdPublico",
            table: "Pedidos",
            column: "IdPublico",
            unique: true);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropIndex(
            name: "IX_Pedidos_IdPublico",
            table: "Pedidos");

        migrationBuilder.DropColumn(
            name: "IdPublico",
            table: "Pedidos");
    }
}
