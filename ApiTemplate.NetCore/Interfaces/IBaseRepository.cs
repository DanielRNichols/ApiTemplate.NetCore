using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace ApiTemplate.NetCore.Interfaces
{
    public interface IBaseRepository<T> where T: class
    {
        Task<IList<T>> GetAll();
        Task<T> GetById(int id);
        Task<bool> Exists(int id);
        Task<bool> Create(T entity);
        Task<bool> Update(T entity);
        Task<bool> Delete(T entity);

        Task<bool> Save();

    }
}
