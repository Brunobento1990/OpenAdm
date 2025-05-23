namespace OpenAdm.Domain.Interfaces;

public interface IParceiroAutenticado
{
    Guid Id { get; set; }
    string ConnectionString { get; set; }
}
