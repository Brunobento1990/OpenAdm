namespace OpenAdm.Domain.Interfaces;

public interface IParceiroAutenticado
{
    string StringConnection { get; set; }   
    string Referer { get; set; }   
    string KeyParceiro { get; set; }   
}
