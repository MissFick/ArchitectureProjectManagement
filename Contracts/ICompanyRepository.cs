using System;
using System.Threading.Tasks;
using ArchitectureProjectManagement.Data;

namespace ArchitectureProjectManagement.Contracts
{
    public interface ICompanyRepository : IGenericRepository<Company>
    {
        //public Task<bool> DoesExist(string companyName, string companyEmail);

        public Task<int> FindIdByDetails(string CompanyName, string companyEmail);
        
    }
}
