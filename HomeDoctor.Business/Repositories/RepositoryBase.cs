using HomeDoctor.Data.DBContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeDoctor.Business.Repositories
{
    public class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        private readonly DbSet<T> _dbSet;
        private readonly DbContext _context;

        public RepositoryBase(HomeDoctorContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
            
        }
        public DbSet<T> GetDbSet()
        {
            return  _dbSet;
        }
        public async Task<bool> Insert(T entity)
        {
            try
            {
                if (entity != null)
                {
                    await _dbSet.AddAsync(entity);
                    return true;
                };
            }catch(Exception e)
            {
                Console.WriteLine("Add Contract EX : "+e.ToString());
            }
            
            return false;
        }

        public async Task<bool> Delete(T entity)
        {
            if(entity != null)
            {
                T exits = await _dbSet.FindAsync(entity);
                if (exits != null)
                {
                    _dbSet.Remove(entity);
                    return true;
                }
                
                return false;
            }
            
            return false;
        }

        public async Task<ICollection<T>> GetAll()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<T> GetById(object id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<bool> Update(T entity)
        {
            if (entity != null)
            {
                _dbSet.Update(entity);
                return true;
            }           
            return false;
        }
        public async Task<bool> UpdateRange(ICollection<T> entitis)
        {
            if (entitis.Any())
            {
                _dbSet.UpdateRange(entitis);
                return true;
            }
            return false;
        }

        public async Task<bool> InsertRange(ICollection<T> entitis)
        {
            if (entitis.Any())
            {
                await _dbSet.AddRangeAsync(entitis);
                return true;
            }
            return false;
        }
    }
}
