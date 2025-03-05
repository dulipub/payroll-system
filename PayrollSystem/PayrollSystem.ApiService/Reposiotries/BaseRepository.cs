using PayrollSystem.ApiService.Core;
using PayrollSystem.ApiService.Models;

namespace PayrollSystem.ApiService.Reposiotries;

public interface IRepository<T> where T : IBaseModel
{
    bool Add(T entity);
    bool Update(T entity);
    bool Delete(T entity);
    T GetById(int id);
}


public class BaseRepository<T>(PayrollDbContext context) : IRepository<T> where T : IBaseModel
{
    public bool Add(T model)
    {
        throw new NotImplementedException();
    }

    public bool Delete(T model)
    {
        throw new NotImplementedException();
    }

    public T GetById(int id)
    {
        throw new NotImplementedException();
    }

    public bool Update(T model)
    {
        throw new NotImplementedException();
    }
}