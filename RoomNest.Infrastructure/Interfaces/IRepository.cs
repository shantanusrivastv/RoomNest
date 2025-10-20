using System.Linq.Expressions;

namespace RoomNest.Infrastructure.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class, new()
    {
        Task<TEntity> AddAsync(TEntity entity);
        Task<bool> Delete<TUniqueType>(TUniqueType uniqueIdentifier);
        Task<TEntity> UpdateAsync(TEntity entity);
        IQueryable<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includes);
        Task<List<TEntity>> FindByAsync(Expression<Func<TEntity, bool>> predicate, bool includeAll = false);
    }
}