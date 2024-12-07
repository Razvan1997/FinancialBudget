using Dollet.Core.Abstractions.Repositories;
using Dollet.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Dollet.Infrastructure.DAL.Repositories
{
    internal class IncomesRepository(DolletDbContext dbContext) : IIncomesRepository
    {
        private readonly DolletDbContext _dbContext = dbContext;

        public void Add(Income income)
        {
            _dbContext.Incomes.Add(income);
        }

        public void Delete(Income income)
        {
            _dbContext.Incomes.Remove(income);
        }

        public async Task DeleteAllAsync(int accountId)
        {
            var toDelete = await _dbContext.Incomes
                .Where(x => x.AccountId == accountId)
                .ToListAsync();

            _dbContext.Incomes.RemoveRange(toDelete);
        }

        public async Task<IEnumerable<Income>> GetAllAsync(DateTime? from = null, DateTime? to = null, int? categoryId = null)
        {
            var query = _dbContext.Incomes
                .Include(x => x.Category)
                .Include(x => x.Account)
                .AsQueryable();

            if (from.HasValue && to.HasValue)
            {
                query = query.Where(x => 
                    x.Date >= from.Value &&
                    x.Date <= to.Value);
            }

            if (categoryId.HasValue)
            {
                query = query.Where(x => x.CategoryId == categoryId.Value);
            }

            return await query.ToListAsync();
        }

        public async Task<Income?> GetAsync(int id)
        {
            return await _dbContext.Incomes.FirstOrDefaultAsync(x => x.Id == id);
        }

        public void Update(Income income)
        {
            _dbContext.Incomes.Update(income);
        }
    }
}
