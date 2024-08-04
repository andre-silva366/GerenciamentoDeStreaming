using GerenciamentoClientesStreaming.Models;

namespace GerenciamentoClientesStreaming.Repositories;

public interface IRepository<T> where T : class
{
    public List<T> Get();
    public T Get(int id);
    public void Post(T obj);
    public void Put(T obj);
    public void Delete(int id);
}
