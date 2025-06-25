using Microsoft.EntityFrameworkCore;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Infra.Context;
using OpenAdm.Domain.Entities;
using Npgsql;
using OpenAdm.Domain.Exceptions;

namespace OpenAdm.Infra.Repositories;

public class UsuarioRepository(ParceiroContext parceiroContext)
    : GenericRepository<Usuario>(parceiroContext), IUsuarioRepository
{
    public override async Task<Usuario> AddAsync(Usuario entity)
    {
        try
        {
            return await base.AddAsync(entity);
        }
        catch (DbUpdateException e)
            when (e.InnerException is NpgsqlException { SqlState: PostgresErrorCodes.UniqueViolation })
        {
            throw new ExceptionApi("Este e-mail, ou cpf ou cnpj já se encontra cadastrado!");
        }
    }
    public async Task<IList<Usuario>> GetAllUsuariosAsync()
    {
        return await ParceiroContext
            .Usuarios
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<int> GetCountCpfAsync()
    {
        try
        {
            return await ParceiroContext
            .Usuarios
            .Where(x => x.Ativo && !string.IsNullOrWhiteSpace(x.Cpf))
            .CountAsync();
        }
        catch (Exception)
        {
            return 0;
        }
    }

    public async Task<int> GetCountCnpjAsync()
    {
        try
        {
            return await ParceiroContext
            .Usuarios
            .Where(x => x.Ativo && !string.IsNullOrWhiteSpace(x.Cnpj))
            .CountAsync();
        }
        catch (Exception)
        {
            return 0;
        }
    }

    public async Task<Usuario?> GetUsuarioByCnpjAsync(string cnpj)
    {
        return await ParceiroContext
             .Usuarios
             .AsNoTracking()
             .FirstOrDefaultAsync(x => x.Cnpj == cnpj);
    }

    public async Task<Usuario?> GetUsuarioByCpfAsync(string cpf)
    {
        return await ParceiroContext
             .Usuarios
             .AsNoTracking()
             .FirstOrDefaultAsync(x => x.Cpf == cpf);
    }

    public async Task<Usuario?> GetUsuarioByEmailAsync(string email)
    {
        return await ParceiroContext
             .Usuarios
             .AsNoTracking()
             .FirstOrDefaultAsync(x => x.Email == email);
    }

    public async Task<Usuario?> GetUsuarioByIdAsync(Guid id)
    {
        return await ParceiroContext
            .Usuarios
            .AsNoTracking()
            .Include(x => x.EnderecoUsuario)
            .Where(x => x.Id == id)
            .Select(x => new Usuario(
                x.Id,
                x.DataDeCriacao,
                x.DataDeAtualizacao,
                x.Numero,
                x.Email,
                x.Senha,
                x.Nome,
                x.Telefone,
                x.Cnpj,
                x.Cpf,
                x.Ativo,
                x.TokenEsqueceuSenha,
                x.DataExpiracaoTokenEsqueceuSenha)
            {
                EnderecoUsuario = x.EnderecoUsuario
            })
            .FirstOrDefaultAsync();
    }

    public async Task AddEnderecoAsync(EnderecoUsuario endereco)
    {
        await ParceiroContext.EnderecoUsuario.AddAsync(endereco);
    }

    public void EditarEndereco(EnderecoUsuario endereco)
    {
        ParceiroContext.EnderecoUsuario.Update(endereco);
    }

    public async Task<EnderecoUsuario?> ObterEnderecoAsync(Guid usuarioId)
    {
        return await ParceiroContext
            .EnderecoUsuario
            .FirstOrDefaultAsync(x => x.UsuarioId == usuarioId);
    }

    public async Task<IList<Usuario>> UsuariosSemPedidoAsync()
    {
        return await ParceiroContext
            .Usuarios
            .AsNoTracking()
            .Where(x => x.Pedidos!.Count == 0)
            .ToListAsync();
    }

    public async Task<Usuario?> GetUsuarioByTokenEsqueceuSenhaAsync(Guid tokenEsqueceuSenha)
    {
        return await ParceiroContext
            .Usuarios
            .FirstOrDefaultAsync(x => x.TokenEsqueceuSenha == tokenEsqueceuSenha && x.Ativo);
    }
}
