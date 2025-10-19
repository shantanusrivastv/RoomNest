﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace RoomNest.Infrastructure
{
    public class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : class, new()
    {
        protected readonly RoomNestDbContext  _context;

        public BaseRepository(RoomNestDbContext context)
        {
            _context = context;
        }

        public IQueryable<TEntity> GetAll()
        {
            try
            {
                return _context.Set<TEntity>();
            }
            catch (Exception)
            {
                throw new Exception("Couldn't retrieve entities");
            }
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            try
            {
                return await _context.Set<TEntity>().ToListAsync();
            }
            catch (Exception)
            {
                throw new Exception("Couldn't retrieve entities");
            }
        }

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException($"{nameof(AddAsync)} entity must not be null");
            }

            try
            {
                await _context.AddAsync(entity);
                await _context.SaveChangesAsync();

                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception($"{nameof(entity)} could not be saved");
            }
        }

        public async Task<IEnumerable<TEntity>> AddListAsync(IEnumerable<TEntity> entities)
        {
            if (entities == null)
            {
                throw new ArgumentNullException($"{nameof(AddAsync)} entity must not be null");
            }

            try
            {
                foreach (var entity in entities)
                {
                    await _context.AddAsync(entity);
                }

                await _context.SaveChangesAsync();
                return entities;
            }
            catch (Exception ex)
            {
                throw new Exception($"{nameof(entities)} could not be saved");
            }
        }

        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException($"{nameof(UpdateAsync)} entity must not be null");
            }

            try
            {
                _context.Update(entity);
                await _context.SaveChangesAsync();
                return entity;
            }
            catch (Exception)
            {
                throw new Exception($"{nameof(entity)} could not be updated");
            }
        }

        public async Task<bool> Delete<TUniqueType>(TUniqueType uniqueIdentifier)
        {
            TEntity entityToDelete = _context.Find<TEntity>(uniqueIdentifier);
            if (entityToDelete == null)
            {
                throw new InvalidOperationException($"{nameof(Delete)} entity not found");
            }

            try
            {
                _context.Remove(entityToDelete);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {
                throw new Exception($"{nameof(TEntity)} could not be deleted");
            }
        }

        //todo: Check if async is better or if IQueryable is more performant
        public IQueryable<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includes)
        {
            var query = _context.Set<TEntity>().Where(predicate);

            return includes.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
        }

        public IQueryable<TEntity> FindByAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return _context.Set<TEntity>().Where(predicate);

        }
    
        public async Task<List<TEntity>> FindByAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includes)
        {
            var query = _context.Set<TEntity>().Where(predicate);
            query = includes.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
            return await query.ToListAsync();
        }

        public async Task<List<TEntity>> FindByAsync(Expression<Func<TEntity, bool>> predicate, bool includeAll = false)
        {
            var query = _context.Set<TEntity>().Where(predicate);

            if (includeAll)
            {
                var navigations = _context.Model.FindEntityType(typeof(TEntity))
                    ?.GetNavigations()
                    .Select(x => x.PropertyInfo.Name)
                    .ToList() ?? new List<string>();

                query = navigations.Aggregate(query, (current, navName) =>
                    current.Include(navName));
            }

            return await query.ToListAsync();
        }

    }

}
