using GerenciamentoClientesStreaming.Models;

namespace GerenciamentoClientesStreaming.Repositories;

public interface IClienteRepository
{
    public List<Cliente> Get();
    public Cliente Get(int id);
    public void Post(Cliente cliente);
    public void Put(Cliente cliente);
    public void Delete(Cliente cliente);
}
