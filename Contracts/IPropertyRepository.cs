using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ArchitectureProjectManagement.Data;

namespace ArchitectureProjectManagement.Contracts
{ 
  public interface IPropertyRepository : IGenericRepository<Property>
  {
        public Task<ICollection<Property>> GetAllByPropertyOwnerId(string id);

     
    }   
}

