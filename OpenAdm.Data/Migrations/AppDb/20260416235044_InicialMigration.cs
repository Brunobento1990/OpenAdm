using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace OpenAdm.Data.Migrations.AppDb
{
    /// <inheritdoc />
    public partial class InicialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AcessosEcommerce",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DataDeCriacao = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Quantidade = table.Column<long>(type: "bigint", nullable: false),
                    ParceiroId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AcessosEcommerce", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Banners",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Foto = table.Column<string>(type: "text", nullable: false),
                    NomeFoto = table.Column<string>(type: "text", nullable: false),
                    Ativo = table.Column<bool>(type: "boolean", nullable: false),
                    DataDeCriacao = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    DataDeAtualizacao = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    Numero = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ParceiroId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Banners", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ConfiguracoesDeFrete",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Token = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    CepOrigem = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: true),
                    Ativo = table.Column<bool>(type: "boolean", nullable: false),
                    CobrarCnpj = table.Column<bool>(type: "boolean", nullable: false),
                    CobrarCpf = table.Column<bool>(type: "boolean", nullable: false),
                    DataDeCriacao = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    DataDeAtualizacao = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    Numero = table.Column<long>(type: "bigint", nullable: false),
                    ParceiroId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConfiguracoesDeFrete", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ConfiguracoesDePagamento",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PublicKey = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    AccessToken = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    UrlWebHook = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    CobrarCpf = table.Column<bool>(type: "boolean", nullable: false),
                    CobrarCnpj = table.Column<bool>(type: "boolean", nullable: false),
                    DataDeCriacao = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    DataDeAtualizacao = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    Numero = table.Column<long>(type: "bigint", nullable: false),
                    ParceiroId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConfiguracoesDePagamento", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ConfiguracoesDePedidos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EmailDeEnvio = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    WhatsApp = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Ativo = table.Column<bool>(type: "boolean", nullable: false),
                    VendaDeProdutoComEstoque = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    PedidoMinimoAtacado = table.Column<decimal>(type: "numeric(12,2)", precision: 12, scale: 2, nullable: true),
                    PedidoMinimoVarejo = table.Column<decimal>(type: "numeric(12,2)", precision: 12, scale: 2, nullable: true),
                    DataDeCriacao = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    DataDeAtualizacao = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    Numero = table.Column<long>(type: "bigint", nullable: false),
                    ParceiroId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConfiguracoesDePedidos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Empresas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Ativo = table.Column<bool>(type: "boolean", nullable: false),
                    UrlEcommerce = table.Column<string>(type: "character varying(350)", maxLength: 350, nullable: false),
                    UrlAdmin = table.Column<string>(type: "character varying(350)", maxLength: 350, nullable: false),
                    ConnectionString = table.Column<string>(type: "text", nullable: false),
                    DataDeCriacao = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    DataDeAtualizacao = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    Numero = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Empresas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Funcionarios",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Senha = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Nome = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Telefone = table.Column<string>(type: "character varying(11)", maxLength: 11, nullable: true),
                    Avatar = table.Column<byte[]>(type: "bytea", nullable: true),
                    Ativo = table.Column<bool>(type: "boolean", nullable: false),
                    DataDeCriacao = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    DataDeAtualizacao = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    Numero = table.Column<long>(type: "bigint", nullable: false),
                    ParceiroId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Funcionarios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LojasParceiras",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    NomeFoto = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Nome = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Foto = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Instagram = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Facebook = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Endereco = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Contato = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    DataDeCriacao = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    DataDeAtualizacao = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    Numero = table.Column<long>(type: "bigint", nullable: false),
                    ParceiroId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LojasParceiras", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TopUsuarios",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TotalCompra = table.Column<decimal>(type: "numeric(12,2)", precision: 12, scale: 2, nullable: false),
                    TotalPedidos = table.Column<int>(type: "integer", nullable: false),
                    UsuarioId = table.Column<Guid>(type: "uuid", nullable: false),
                    Usuario = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    DataDeCriacao = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    DataDeAtualizacao = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    Numero = table.Column<long>(type: "bigint", nullable: false),
                    ParceiroId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TopUsuarios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Parceiros",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EmpresaOpenAdmId = table.Column<Guid>(type: "uuid", nullable: false),
                    RazaoSocial = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Logo = table.Column<byte[]>(type: "bytea", nullable: true),
                    NomeFantasia = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Cnpj = table.Column<string>(type: "character varying(14)", maxLength: 14, nullable: false),
                    DataDeCriacao = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    DataDeAtualizacao = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    Numero = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parceiros", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Parceiros_Empresas_EmpresaOpenAdmId",
                        column: x => x.EmpresaOpenAdmId,
                        principalTable: "Empresas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EnderecoParceiro",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ParceiroId = table.Column<Guid>(type: "uuid", nullable: false),
                    Cep = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: false),
                    Logradouro = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Bairro = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Localidade = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Complemento = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Numero = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    Uf = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnderecoParceiro", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EnderecoParceiro_Parceiros_ParceiroId",
                        column: x => x.ParceiroId,
                        principalTable: "Parceiros",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RedesSociais",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Link = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    RedeSocialEnum = table.Column<int>(type: "integer", nullable: false),
                    ParceiroId = table.Column<Guid>(type: "uuid", nullable: false),
                    DataDeCriacao = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    DataDeAtualizacao = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    Numero = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RedesSociais", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RedesSociais_Parceiros_ParceiroId",
                        column: x => x.ParceiroId,
                        principalTable: "Parceiros",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TelefonesParceiro",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ParceiroId = table.Column<Guid>(type: "uuid", nullable: false),
                    Telefone = table.Column<string>(type: "character varying(14)", maxLength: 14, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TelefonesParceiro", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TelefonesParceiro_Parceiros_ParceiroId",
                        column: x => x.ParceiroId,
                        principalTable: "Parceiros",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AcessosEcommerce_DataDeCriacao",
                table: "AcessosEcommerce",
                column: "DataDeCriacao");

            migrationBuilder.CreateIndex(
                name: "IX_AcessosEcommerce_ParceiroId",
                table: "AcessosEcommerce",
                column: "ParceiroId");

            migrationBuilder.CreateIndex(
                name: "IX_Banners_Numero",
                table: "Banners",
                column: "Numero");

            migrationBuilder.CreateIndex(
                name: "IX_Banners_ParceiroId",
                table: "Banners",
                column: "ParceiroId");

            migrationBuilder.CreateIndex(
                name: "IX_ConfiguracoesDeFrete_Numero",
                table: "ConfiguracoesDeFrete",
                column: "Numero");

            migrationBuilder.CreateIndex(
                name: "IX_ConfiguracoesDeFrete_ParceiroId",
                table: "ConfiguracoesDeFrete",
                column: "ParceiroId");

            migrationBuilder.CreateIndex(
                name: "IX_ConfiguracoesDePagamento_Numero",
                table: "ConfiguracoesDePagamento",
                column: "Numero");

            migrationBuilder.CreateIndex(
                name: "IX_ConfiguracoesDePagamento_ParceiroId",
                table: "ConfiguracoesDePagamento",
                column: "ParceiroId");

            migrationBuilder.CreateIndex(
                name: "IX_ConfiguracoesDePedidos_Numero",
                table: "ConfiguracoesDePedidos",
                column: "Numero");

            migrationBuilder.CreateIndex(
                name: "IX_ConfiguracoesDePedidos_ParceiroId",
                table: "ConfiguracoesDePedidos",
                column: "ParceiroId");

            migrationBuilder.CreateIndex(
                name: "IX_Empresas_Ativo",
                table: "Empresas",
                column: "Ativo");

            migrationBuilder.CreateIndex(
                name: "IX_Empresas_UrlAdmin",
                table: "Empresas",
                column: "UrlAdmin");

            migrationBuilder.CreateIndex(
                name: "IX_Empresas_UrlAdmin_UrlEcommerce",
                table: "Empresas",
                columns: new[] { "UrlAdmin", "UrlEcommerce" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Empresas_UrlAdmin_UrlEcommerce_Ativo",
                table: "Empresas",
                columns: new[] { "UrlAdmin", "UrlEcommerce", "Ativo" });

            migrationBuilder.CreateIndex(
                name: "IX_Empresas_UrlEcommerce",
                table: "Empresas",
                column: "UrlEcommerce");

            migrationBuilder.CreateIndex(
                name: "IX_EnderecoParceiro_ParceiroId",
                table: "EnderecoParceiro",
                column: "ParceiroId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Funcionarios_Email_ParceiroId",
                table: "Funcionarios",
                columns: new[] { "Email", "ParceiroId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Funcionarios_Numero",
                table: "Funcionarios",
                column: "Numero");

            migrationBuilder.CreateIndex(
                name: "IX_Funcionarios_ParceiroId",
                table: "Funcionarios",
                column: "ParceiroId");

            migrationBuilder.CreateIndex(
                name: "IX_LojasParceiras_Numero",
                table: "LojasParceiras",
                column: "Numero");

            migrationBuilder.CreateIndex(
                name: "IX_LojasParceiras_ParceiroId",
                table: "LojasParceiras",
                column: "ParceiroId");

            migrationBuilder.CreateIndex(
                name: "IX_Parceiros_EmpresaOpenAdmId",
                table: "Parceiros",
                column: "EmpresaOpenAdmId");

            migrationBuilder.CreateIndex(
                name: "IX_RedesSociais_ParceiroId",
                table: "RedesSociais",
                column: "ParceiroId");

            migrationBuilder.CreateIndex(
                name: "IX_TelefonesParceiro_ParceiroId",
                table: "TelefonesParceiro",
                column: "ParceiroId");

            migrationBuilder.CreateIndex(
                name: "IX_TopUsuarios_Numero",
                table: "TopUsuarios",
                column: "Numero");

            migrationBuilder.CreateIndex(
                name: "IX_TopUsuarios_ParceiroId",
                table: "TopUsuarios",
                column: "ParceiroId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AcessosEcommerce");

            migrationBuilder.DropTable(
                name: "Banners");

            migrationBuilder.DropTable(
                name: "ConfiguracoesDeFrete");

            migrationBuilder.DropTable(
                name: "ConfiguracoesDePagamento");

            migrationBuilder.DropTable(
                name: "ConfiguracoesDePedidos");

            migrationBuilder.DropTable(
                name: "EnderecoParceiro");

            migrationBuilder.DropTable(
                name: "Funcionarios");

            migrationBuilder.DropTable(
                name: "LojasParceiras");

            migrationBuilder.DropTable(
                name: "RedesSociais");

            migrationBuilder.DropTable(
                name: "TelefonesParceiro");

            migrationBuilder.DropTable(
                name: "TopUsuarios");

            migrationBuilder.DropTable(
                name: "Parceiros");

            migrationBuilder.DropTable(
                name: "Empresas");
        }
    }
}
