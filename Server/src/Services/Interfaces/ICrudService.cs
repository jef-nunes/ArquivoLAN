namespace Server.Services.Interfaces;

public interface ICrudService<T>
{
    T? FindById(long id);
    List<T> FindAll();
    T Update(long id, T entity);
    void Delete(long id);
}