using Dollet.Core.Entities;

namespace Dollet.Core.Abstractions.Repositories
{
    public interface IIncomesRepository
    {
        Task<IEnumerable<Income>> GetAllAsync(DateTime? from = null, DateTime? to = null, int? categoryId = null);
        Task<Income?> GetAsync(int id);
        void Add(Income income);
        void Update(Income income);
        void Delete(Income income);
        Task DeleteAllAsync(int accountId);
    }
}