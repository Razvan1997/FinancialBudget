using Dollet.Core.Entities;

namespace Dollet.Core.Abstractions.Repositories
{
    public interface IAccountRepository
    {
        Task<Account?> GetAsync(int id);
        Task<IEnumerable<Account>> GetAsyncByUserId(int userId);
        Task<IEnumerable<Account>> GetAsyncByUserAndPass(string username, string password);
        Task<Account?> GetDefaultAsync();
        Task<IEnumerable<Account>> GetAllAsync();
        Task<bool> ExistsAsync();
        void Add(Account account);
        void Delete(Account account);
        void Update(Account account);
        Task<Account> GetByIdAsync(int id);
    }
}
