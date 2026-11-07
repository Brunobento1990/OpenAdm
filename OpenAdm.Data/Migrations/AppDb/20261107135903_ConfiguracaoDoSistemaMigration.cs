using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OpenAdm.Data.Migrations.AppDb
{
    /// <inheritdoc />
    public partial class ConfiguracaoDoSistemaMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "ConfiguracoesDePedidos",
                newName: "ConfiguracaoDoSistema");

            migrationBuilder.RenameColumn(
                name: "EmailDeEnvio",
                table: "ConfiguracaoDoSistema",
                newName: "EmailParaEnvioNotificacao");

            migrationBuilder.RenameColumn(
                name: "WhatsApp",
                table: "ConfiguracaoDoSistema",
                newName: "WhatsAppParaEnvioNotificacao");

            migrationBuilder.RenameIndex(
                name: "IX_ConfiguracoesDePedidos_Ativo_ParceiroId",
                table: "ConfiguracaoDoSistema",
                newName: "IX_ConfiguracaoDoSistema_Ativo_ParceiroId");

            migrationBuilder.RenameIndex(
                name: "IX_ConfiguracoesDePedidos_Numero",
                table: "ConfiguracaoDoSistema",
                newName: "IX_ConfiguracaoDoSistema_Numero");

            migrationBuilder.RenameIndex(
                name: "IX_ConfiguracoesDePedidos_ParceiroId",
                table: "ConfiguracaoDoSistema",
                newName: "IX_ConfiguracaoDoSistema_ParceiroId");

            migrationBuilder.Sql(
                "ALTER TABLE \"ConfiguracaoDoSistema\" RENAME CONSTRAINT \"PK_ConfiguracoesDePedidos\" TO \"PK_ConfiguracaoDoSistema\";");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                "ALTER TABLE \"ConfiguracaoDoSistema\" RENAME CONSTRAINT \"PK_ConfiguracaoDoSistema\" TO \"PK_ConfiguracoesDePedidos\";");

            migrationBuilder.RenameIndex(
                name: "IX_ConfiguracaoDoSistema_Ativo_ParceiroId",
                table: "ConfiguracaoDoSistema",
                newName: "IX_ConfiguracoesDePedidos_Ativo_ParceiroId");

            migrationBuilder.RenameIndex(
                name: "IX_ConfiguracaoDoSistema_Numero",
                table: "ConfiguracaoDoSistema",
                newName: "IX_ConfiguracoesDePedidos_Numero");

            migrationBuilder.RenameIndex(
                name: "IX_ConfiguracaoDoSistema_ParceiroId",
                table: "ConfiguracaoDoSistema",
                newName: "IX_ConfiguracoesDePedidos_ParceiroId");

            migrationBuilder.RenameColumn(
                name: "EmailParaEnvioNotificacao",
                table: "ConfiguracaoDoSistema",
                newName: "EmailDeEnvio");

            migrationBuilder.RenameColumn(
                name: "WhatsAppParaEnvioNotificacao",
                table: "ConfiguracaoDoSistema",
                newName: "WhatsApp");

            migrationBuilder.RenameTable(
                name: "ConfiguracaoDoSistema",
                newName: "ConfiguracoesDePedidos");
        }
    }
}
