using PayrollSystem.ApiService.Core;
using PayrollSystem.ApiService.Models;
using static Grpc.Core.Metadata;
using System.Threading;

namespace PayrollSystem.ApiService.Repositories;

public interface IRepository<TEntity> where TEntity : class, IBaseModel
{
    Task<bool> Add(TEntity entity);
    Task<bool> Update(TEntity entity);
    Task<bool> Delete(TEntity entity);
    Task<TEntity?> GetById(int id);
}


public class BaseRepository<TEntity>(PayrollDbContext context) : IRepository<TEntity> where TEntity : class, IBaseModel
{
    public async Task<bool> Add(TEntity entity)
    {
        await context.Set<TEntity>().AddAsync(entity);
        var sucess = await context.SaveChangesAsync();
        return sucess > 0;
    }

    public async Task<bool> Delete(TEntity entity)
    {
        context.Set<TEntity>().Remove(entity);
        var sucess = await context.SaveChangesAsync();
        return sucess > 0;
    }

    public async Task<TEntity?> GetById(int id)
    {
        return await context.Set<TEntity>().FindAsync(id);
    }

    public async Task<bool> Update(TEntity entity)
    {
        var existingEntity = GetById(entity.Id);
        if (Equals(existingEntity, default(TEntity)))
            return false;

        context.Entry(existingEntity).CurrentValues.SetValues(entity);
        var sucess = await context.SaveChangesAsync();
        return sucess > 0;
    }
}