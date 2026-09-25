using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OpenAdm.Data.Migrations.AppDb
{
    /// <inheritdoc />
    public partial class TipoUsuarioMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SessoesUsuarios_UsuarioId_ParceiroId_EhFuncionario_Revogado~",
                table: "SessoesUsuarios");

            migrationBuilder.DropColumn(
                name: "EhFuncionario",
                table: "SessoesUsuarios");

            migrationBuilder.AddColumn<int>(
                name: "TipoUsuario",
                table: "SessoesUsuarios",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_SessoesUsuarios_UsuarioId_ParceiroId_TipoUsuario_RevogadoEm~",
                table: "SessoesUsuarios",
                columns: new[] { "UsuarioId", "ParceiroId", "TipoUsuario", "RevogadoEm", "ExpiraEm" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SessoesUsuarios_UsuarioId_ParceiroId_TipoUsuario_RevogadoEm~",
                table: "SessoesUsuarios");

            migrationBuilder.DropColumn(
                name: "TipoUsuario",
                table: "SessoesUsuarios");

            migrationBuilder.AddColumn<bool>(
                name: "EhFuncionario",
                table: "SessoesUsuarios",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_SessoesUsuarios_UsuarioId_ParceiroId_EhFuncionario_Revogado~",
                table: "SessoesUsuarios",
                columns: new[] { "UsuarioId", "ParceiroId", "EhFuncionario", "RevogadoEm", "ExpiraEm" });
        }
    }
}
