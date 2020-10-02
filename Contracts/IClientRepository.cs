using System;
using ArchitectureProjectManagement.Data;

namespace ArchitectureProjectManagement.Contracts
{
    public interface IClientRepository : IGenericRepository<Client>
    {
        public int FindIdByDetails(string id);
    }
}
