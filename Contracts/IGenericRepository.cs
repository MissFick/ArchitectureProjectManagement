using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ArchitectureProjectManagement.Data;

namespace ArchitectureProjectManagement.Contracts
{
        public interface IGenericRepository<T> where T : class
        {
            Task<bool> Add(T entity);
            Task<T> Get(int id);
            Task<ICollection<T>> GetAll();
            Task<bool> Delete(T entity);
            Task<bool> Update(T entity);
            Task<bool> DoesExist(string id);
            Task<bool> Save();
    }
}
