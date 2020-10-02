using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ArchitectureProjectManagement.Contracts;
using ArchitectureProjectManagement.Data;
using Microsoft.EntityFrameworkCore;
using cloudscribe.Core.Identity;

namespace ArchitectureProjectManagement.Repository
{
    public class ClientRepository : IClientRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public ClientRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> Add(Client entity)
        {
            await _dbContext.tb_Client.AddAsync(entity);
            return await Save();
        }

        public async Task<bool> DoesExist(string id)
        {
            return true;
        }

        public async Task<bool> Delete(Client entity)
        {
            _dbContext.tb_Client.Remove(entity);
            return await Save();
        }

        public async Task<Client> Get(int id)
        {
            var client = _dbContext.tb_Client
                .Include(q => q.ApplicationUser)
                .Include(q => q.ApplicationUser.IdentityUser)
                .Include(q => q.Company)
                .Where(q => q.ClientId == id)
                .FirstOrDefaultAsync();
            return await client;
        }

        public async Task<ICollection<Client>> GetAll()
        {
            var clients = _dbContext.tb_Client
                .Include(q => q.ApplicationUser)
                .Include(q => q.ApplicationUser.IdentityUser)
                .ToListAsync();
            return await clients;
        }


        /*        public bool DoesExist(string erfLotNo)
                {
                    var exists = GetAll().Where(n => n.ERF_LotNo == erfLotNo)
                                    .Any();
                    return exists;
                }
        */

        public int FindIdByDetails(string id)
        {
            var client = _dbContext.tb_Client
                .Where(n => n.ApplicationUser.Id == id)
                .FirstOrDefaultAsync();
            var clientidentity = client.Result.ClientId;
            return clientidentity;
        }

        public async Task<bool> Update(Client entity)
        {
            _dbContext.tb_Client.Update(entity);
            return await Save();
        }

        public async Task<bool> Save()
        {
            var changes = await _dbContext.SaveChangesAsync();
            return changes > 0;
        }
    }
}
