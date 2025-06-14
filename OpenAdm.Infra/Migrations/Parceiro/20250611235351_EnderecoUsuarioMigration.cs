using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OpenAdm.Infra.Migrations.Parceiro
{
    /// <inheritdoc />
    public partial class EnderecoUsuarioMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Logo",
                table: "ConfiguracoesDePedidos");

            migrationBuilder.CreateTable(
                name: "EnderecoUsuario",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UsuarioId = table.Column<Guid>(type: "uuid", nullable: false),
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
                    table.PrimaryKey("PK_EnderecoUsuario", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EnderecoUsuario_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EnderecoUsuario_UsuarioId",
                table: "EnderecoUsuario",
                column: "UsuarioId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EnderecoUsuario");

            migrationBuilder.AddColumn<byte[]>(
                name: "Logo",
                table: "ConfiguracoesDePedidos",
                type: "bytea",
                nullable: true);
        }
    }
}
