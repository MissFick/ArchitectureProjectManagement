using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ArchitectureProjectManagement.Contracts;
using ArchitectureProjectManagement.Data;
using Microsoft.EntityFrameworkCore;

namespace ArchitectureProjectManagement.Repository
{
    public class PropertyRepository : IPropertyRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public PropertyRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> Add(Property entity)
        {
            await _dbContext.tb_Property.AddAsync(entity);
            return await Save();
        }

        public async Task<bool> Delete(Property entity)
        {
            _dbContext.tb_Property.Remove(entity);
            return await Save();
        }

        public async Task<Property> Get(int id)
        {
            var property = await _dbContext.tb_Property.FindAsync(id);
            return property;
        }

        public async Task<ICollection<Property>> GetAll()
        {
            var properties = await _dbContext.tb_Property.ToListAsync();
            return properties;
        }

        public async Task<ICollection<Property>> GetAllByPropertyOwnerId(string id)
        {
            var properties = await _dbContext.tb_Property
                .Where(n => n.PropertyOwnerId == id)
                .ToListAsync();
            return properties;
        }


        public async Task<bool> Update(Property entity)
        {
            _dbContext.tb_Property.Update(entity);
            return await Save();
        }

        public async Task<bool> Save()
        {
            var changes = await _dbContext.SaveChangesAsync();
            return changes > 0;
        }

       /* public ICollection<Property> GetAllByClientId(int id)
        {
            throw new NotImplementedException();
        }*/

        public Task<bool> DoesExist(string id)
        {
            throw new NotImplementedException();
        }
    }
}
