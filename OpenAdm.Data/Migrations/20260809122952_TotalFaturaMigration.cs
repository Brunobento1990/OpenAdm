using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OpenAdm.Data.Migrations;

public partial class TotalFaturaMigration : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<decimal>(
            name: "Total",
            table: "Faturas",
            type: "numeric(12,2)",
            precision: 12,
            scale: 2,
            nullable: false,
            defaultValue: 0m);

        migrationBuilder.Sql(
            """
            UPDATE "Faturas" AS f
            SET "Total" = COALESCE((
                SELECT SUM(p."Valor")
                FROM "Parcelas" AS p
                WHERE p."FaturaId" = f."Id"
            ), 0)
            """);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "Total",
            table: "Faturas");
    }
}
