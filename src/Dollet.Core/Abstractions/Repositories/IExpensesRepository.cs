using Dollet.Core.Entities;

namespace Dollet.Core.Abstractions.Repositories
{
    public interface IExpensesRepository
    {
        Task<IEnumerable<Expense>> GetAllAsync(DateTime? from = null, DateTime? to = null, int? categoryId = null, int? accountId = null);
        Task<Expense?> GetAsync(int id);
        void Add(Expense expense);
        void Update(Expense expense);
        void Delete(Expense expense);
        Task DeleteAllAsync(int accountId);
    }
}