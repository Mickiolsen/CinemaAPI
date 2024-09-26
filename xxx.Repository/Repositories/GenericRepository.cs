using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using xxx.Repository.Models;
using xxx.Repository.Interfaces;

namespace Cinema.Repository.Repositories
{
    public class GenericRepository<T> : ICrudRepository<T> where T : class, IEntity
    {
        public DataContext context { get; set; }
        public GenericRepository(DataContext database) { context = database; }

        public async Task<IEnumerable<T>> GetAll()
        {
            var exists = await context.Set<T>().ToListAsync();
            return exists;
        }
        public async Task<T?> GetById(int id)
        {
            var result = await context.Set<T>().AsNoTracking().FirstOrDefaultAsync(e => e.Id == id);
            return result;
        }

        public async Task<T> Create(T entity)
        {
            try
            {
                context.Set<T>().Add(entity);
                await context.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {
                return null;
            } 
        }

        public async Task<bool> DeleteById(int id) 
        {
            var entityToDelete = await context.Set<T>().FirstOrDefaultAsync(e => e.Id == id); 
            if (entityToDelete != null)
            {
                context.Set<T>().Remove(entityToDelete); 
                await context.SaveChangesAsync(); 
                return true;
            }
            return false;
        }
    }
}
