using System.Linq.Expressions;
using Ecommerce.DataAccess.Data;
using Ecommerce.DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.DataAccess.Repository;

public class Repository<T>: IRepository<T> where T : class
{
    private readonly ApplicationDbContext _dbContext;
    private readonly DbSet<T> _dbSet;

    protected Repository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
        this._dbSet = _dbContext.Set<T>();
    }
    public IEnumerable<T> GetAll()
    {
        IQueryable<T> query = _dbSet;
        return query.ToList();
    }

    public T Get(Expression<Func<T, bool>> predicate)
    {
        IQueryable<T> query = _dbSet;
        query = query.Where(predicate);
        return query.FirstOrDefault();
    }

    public void Add(T entity)
    {
        _dbSet.Add(entity);
    }

    public void Remove(T entity)
    {
        _dbSet.Remove(entity);
    }

    public void RemoveRange(IEnumerable<T> entities)
    {
        _dbSet.RemoveRange(entities);
    }
}