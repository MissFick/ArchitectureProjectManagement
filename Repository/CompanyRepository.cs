using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ArchitectureProjectManagement.Contracts;
using ArchitectureProjectManagement.Data;

namespace ArchitectureProjectManagement.Repository
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public CompanyRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> Add(Company entity)
        {
            await _dbContext.tb_Company.AddAsync(entity);
            return await Save();
        }

        
         public async Task<bool> DoesExist(string companyEmail)
        {
            var exists = await _dbContext.tb_Company
                            .Where(n => n.CompanyEmail == companyEmail)
                            .AnyAsync();
            return exists;
        }

       
        public async Task<int> FindIdByDetails(string companyName, string companyEmail)
        {
            var companies = await _dbContext.tb_Company
                .Where(n=> n.CompanyName == companyName && n.CompanyEmail == companyEmail)
                .FirstOrDefaultAsync();
            return companies.CompanyId;
        }

        public async Task<bool> Delete(Company entity)
        {
            _dbContext.tb_Company.Remove(entity);
            return await Save();
        }

        public async Task<Company> Get(int id)
        {
            var company = await _dbContext.tb_Company.FindAsync(id);
            return company;
        }

        public async Task<ICollection<Company>> GetAll()
        {
            var companies = await _dbContext.tb_Company.ToListAsync();
            return companies;
        }

        public async Task<bool> Update(Company entity)
        {
            _dbContext.tb_Company.Update(entity);
                return await Save();
        }

        public async Task<bool> Save()
        {
            var changes = await _dbContext.SaveChangesAsync();
            return changes > 0;
        }
    }
}
