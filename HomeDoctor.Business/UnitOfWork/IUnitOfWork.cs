using HomeDoctor.Business.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HomeDoctor.Business.UnitOfWork
{
    public interface IUnitOfWork
    {
        // get repository of any model
        public IRepositoryBase<T> GetRepository<T>() where T : class;
        public Task CommitAsync();
        public Task DisposeAsync();


    }
}
