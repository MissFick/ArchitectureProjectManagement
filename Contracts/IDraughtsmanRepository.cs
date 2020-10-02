using System;
using System.Threading.Tasks;
using ArchitectureProjectManagement.Data;

namespace ArchitectureProjectManagement.Contracts
{
    public interface IDraughtsmanRepository : IGenericRepository<Draughtsman>
    {
        //public bool DoesExist(string draughtsmanregno);

        public Task<int> FindIdByDetails(string draughtsmanregnol);

        public Task<int> FindDraughtsmanId(string id);
    }
}
