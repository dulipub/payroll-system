﻿using PayrollSystem.ApiService.Core;
using PayrollSystem.ApiService.Models;
using static Grpc.Core.Metadata;
using System.Threading;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Linq;

namespace PayrollSystem.ApiService.Repositories;

public interface IRepository<TEntity> where TEntity : class, IBaseModel
{
    Task<bool> Add(TEntity entity);
    Task<bool> Update(TEntity entity);
    Task<bool> Delete(TEntity entity);
    Task<TEntity?> GetById(int id);

    Task<IEnumerable<TEntity>> ListAscending( Expression<Func<TEntity, bool>> filter = null, Expression<Func<TEntity, object>> orderBy = null,
        string includeProperties = "");

    Task<IEnumerable<TEntity>> ListDescending( Expression<Func<TEntity, bool>> filter = null, Expression<Func<TEntity, object>> orderBy = null,
        string includeProperties = "");
}


public class BaseRepository<TEntity>(PayrollDbContext context) : IRepository<TEntity> where TEntity : class, IBaseModel
{
    public virtual async Task<bool> Add(TEntity entity)
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

    public virtual async Task<IEnumerable<TEntity>> ListAscending(
        Expression<Func<TEntity, bool>> filter = null,
        Expression<Func<TEntity, object>> orderBy = null,
        string includeProperties = "")
    {
        IQueryable<TEntity> query = context.Set<TEntity>();
        query = await GetFilterQuery(query, filter);
        query = await GetIncludesQuery(query, includeProperties);
        query = query.OrderBy(orderBy);

        return await query.ToListAsync();
    }

    public virtual async Task<IEnumerable<TEntity>> ListDescending(
        Expression<Func<TEntity, bool>> filter = null,
        Expression<Func<TEntity, object>> orderBy = null,
        string includeProperties = "")
    {
        IQueryable<TEntity> query = context.Set<TEntity>();
        query = await GetFilterQuery(query, filter);
        query = await GetIncludesQuery(query, includeProperties);
        query = query.OrderByDescending(orderBy);

        return await query.ToListAsync();
    }

    protected virtual async Task<IQueryable<TEntity>> GetFilterQuery(
        IQueryable<TEntity> query,
        Expression<Func<TEntity, bool>> filter = null)
    {
        if (filter != null)
        {
            query = query.Where(filter);
        }
        return query;
    }

    protected virtual async Task<IQueryable<TEntity>> GetIncludesQuery(
        IQueryable<TEntity> query,
        string includeProperties = "")
    {
        foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
        {
            query = query.Include(includeProperty);
        }

        return query;
    }
}