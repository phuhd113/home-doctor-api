using HomeDoctor.Business.Repositories;
using HomeDoctor.Data.DBContext;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HomeDoctor.Business.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly HomeDoctorContext _context;
        private readonly Dictionary<Type, object> repositories;

        public UnitOfWork(HomeDoctorContext context)
        {
            _context = context;
            repositories = new Dictionary<Type, object>();
        }
        public async Task CommitAsync()
        {
           await _context.SaveChangesAsync();
        }
     
        public async Task DisposeAsync()
        {
            await _context.DisposeAsync();
        }

        public IRepositoryBase<T> GetRepository<T>() where T : class
        {
            var type = typeof(T);
            if (!repositories.ContainsKey(type))
            {
                var repositoryType = typeof(RepositoryBase<>);
                var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(T)), _context);
                repositories.Add(type, repositoryInstance);
            }
            return (IRepositoryBase<T>)repositories[type];
        }
    }
}
