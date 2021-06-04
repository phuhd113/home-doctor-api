using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeDoctor.Business.Repositories
{
    public interface IRepositoryBase<T> where T : class
    {
        public Task<bool> Insert(T entity);
        public Task<bool> InsertRange(ICollection<T> entity);
        public Task<bool> Update(T entity);
        public Task<bool> Delete(T entity);
        public Task<T> GetById(object id);
        public Task<ICollection<T>> GetAll();
        public Task<bool> UpdateRange(ICollection<T> entitis);
        public DbSet<T> GetDbSet();
    }
}
