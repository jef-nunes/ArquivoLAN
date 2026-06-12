namespace Server.Services;

public interface ICrudService<T>
{
    T Create(T entity);
    T? FindById(long id);
    List<T> FindAll();
    T Update(long id, T entity);
    void Delete(long id);
}