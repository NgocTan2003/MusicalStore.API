using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicalStore.Application.Repositories.RepositoryBase
{
    public interface IRepositoryBase
    {
        public interface IRepositoryBase<T> where T : class
        {
            IQueryable<T> GetAll();
            Task<bool> Exists(Guid Id);
            Task<T> FindById(Guid Id);
            Task<int> Create(T entity);
            Task<int> Update(T entity);
            Task<int> Delete(Guid Id);
            Task<List<T>> Pagination(int page = 1);
        }
    }
}
