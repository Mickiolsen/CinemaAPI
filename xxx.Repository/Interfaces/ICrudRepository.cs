using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xxx.Repository.Models;

namespace xxx.Repository.Interfaces
{

    public interface ICrudRepository<T> where T : class
    {
        public Task<IEnumerable<T>> GetAll();
        public Task<T?> GetById(int id);
        public Task<T> Create(T entity);
        public Task<bool> DeleteById(int id);
    }
}
