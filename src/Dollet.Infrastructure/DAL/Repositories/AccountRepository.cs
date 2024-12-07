using Dollet.Core.Abstractions.Repositories;
using Dollet.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Dollet.Infrastructure.DAL.Repositories
{
    internal class AccountRepository(DolletDbContext dbContext) : IAccountRepository
    {
        private readonly DbSet<Account> _accounts = dbContext.Accounts;

        public void Add(Account account)
        {
            _accounts.Add(account);
        }

        public void Delete(Account account)
        {
            _accounts.Remove(account);
        }

        public async Task<bool> ExistsAsync()
        {
            return await _accounts.AnyAsync();
        }

        public async Task<IEnumerable<Account>> GetAllAsync()
        {
            return await _accounts
                .OrderByDescending(x => x.IsDefault)
                .ToListAsync();
        }

        public async Task<Account?> GetAsync(int id)
        {
            return await _accounts.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<Account>> GetAsyncByUserId(int userId)
        {
            return await _accounts.Where(x => x.UserId == userId).ToListAsync();
        }

        public async Task<IEnumerable<Account>> GetAsyncByUserAndPass(string username, string password)
        {
            return await _accounts.Where(x => x.Username == username && x.Password == password).ToListAsync();
        }

        public async Task<Account?> GetDefaultAsync()
        {
            return await _accounts.FirstOrDefaultAsync(x => x.IsDefault);
        }

        public void Update(Account account)
        {
            _accounts.Update(account);
        }
    }
}
