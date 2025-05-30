﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace OpenAdm.Infra.Migrations.Parceiro
{
    /// <inheritdoc />
    public partial class Faturasmigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FaturasContasAReceber");

            migrationBuilder.DropTable(
                name: "ContasAReceber");

            migrationBuilder.CreateTable(
                name: "Faturas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    Tipo = table.Column<int>(type: "integer", nullable: false),
                    UsuarioId = table.Column<Guid>(type: "uuid", nullable: false),
                    PedidoId = table.Column<Guid>(type: "uuid", nullable: true),
                    DataDeFechamento = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    DataDeCriacao = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    DataDeAtualizacao = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    Numero = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Faturas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Faturas_Pedidos_PedidoId",
                        column: x => x.PedidoId,
                        principalTable: "Pedidos",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Faturas_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Parcelas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    DataDeVencimento = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    DataDePagamento = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    NumeroDaFatura = table.Column<int>(type: "integer", nullable: false),
                    MeioDePagamento = table.Column<int>(type: "integer", nullable: true),
                    Valor = table.Column<decimal>(type: "numeric(12,2)", precision: 12, scale: 2, nullable: false),
                    Desconto = table.Column<decimal>(type: "numeric(12,2)", precision: 12, scale: 2, nullable: true),
                    Observacao = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    IdExterno = table.Column<string>(type: "text", nullable: true),
                    FaturaId = table.Column<Guid>(type: "uuid", nullable: false),
                    DataDeCriacao = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    DataDeAtualizacao = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    Numero = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parcelas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Parcelas_Faturas_FaturaId",
                        column: x => x.FaturaId,
                        principalTable: "Faturas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Faturas_PedidoId",
                table: "Faturas",
                column: "PedidoId");

            migrationBuilder.CreateIndex(
                name: "IX_Faturas_Tipo",
                table: "Faturas",
                column: "Tipo");

            migrationBuilder.CreateIndex(
                name: "IX_Faturas_UsuarioId",
                table: "Faturas",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Parcelas_FaturaId",
                table: "Parcelas",
                column: "FaturaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Parcelas");

            migrationBuilder.DropTable(
                name: "Faturas");

            migrationBuilder.CreateTable(
                name: "ContasAReceber",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PedidoId = table.Column<Guid>(type: "uuid", nullable: true),
                    UsuarioId = table.Column<Guid>(type: "uuid", nullable: false),
                    DataDeAtualizacao = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    DataDeCriacao = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    DataDeFechamento = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Numero = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContasAReceber", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContasAReceber_Pedidos_PedidoId",
                        column: x => x.PedidoId,
                        principalTable: "Pedidos",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ContasAReceber_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FaturasContasAReceber",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ContasAReceberId = table.Column<Guid>(type: "uuid", nullable: false),
                    DataDeAtualizacao = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    DataDeCriacao = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    DataDePagamento = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    DataDeVencimento = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Desconto = table.Column<decimal>(type: "numeric(12,2)", precision: 12, scale: 2, nullable: true),
                    IdExterno = table.Column<string>(type: "text", nullable: true),
                    MeioDePagamento = table.Column<int>(type: "integer", nullable: true),
                    Numero = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NumeroDaFatura = table.Column<int>(type: "integer", nullable: false),
                    Observacao = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    Valor = table.Column<decimal>(type: "numeric(12,2)", precision: 12, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FaturasContasAReceber", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FaturasContasAReceber_ContasAReceber_ContasAReceberId",
                        column: x => x.ContasAReceberId,
                        principalTable: "ContasAReceber",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ContasAReceber_PedidoId",
                table: "ContasAReceber",
                column: "PedidoId");

            migrationBuilder.CreateIndex(
                name: "IX_ContasAReceber_UsuarioId",
                table: "ContasAReceber",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_FaturasContasAReceber_ContasAReceberId",
                table: "FaturasContasAReceber",
                column: "ContasAReceberId");
        }
    }
}
