using Dollet.Core.Abstractions.Repositories;
using Dollet.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Dollet.Infrastructure.DAL.Repositories
{
    internal class ExpensesRepository(DolletDbContext dbContext) : IExpensesRepository
    {
        private readonly DolletDbContext _dbContext = dbContext;

        public void Add(Expense expense)
        {
            _dbContext.Expenses.Add(expense);
        }

        public void Delete(Expense expense)
        {
            _dbContext.Expenses.Remove(expense);
        }

        public async Task DeleteAllAsync(int accountId)
        {
            var toDelete = await _dbContext.Expenses
                .Where(x => x.AccountId == accountId)
                .ToListAsync();

            _dbContext.Expenses.RemoveRange(toDelete);
        }

        public async Task<IEnumerable<Expense>> GetAllAsync(DateTime? from = null, DateTime? to = null, int? categoryId = null, int? accountId = null)
        {
            var query = _dbContext.Expenses
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

            if (accountId.HasValue)
            {
                query = query.Where(x => x.AccountId == accountId.Value);
            }

            return await query.ToListAsync();
        }

        public async Task<Expense?> GetAsync(int id)
        {
            return await _dbContext.Expenses.FirstOrDefaultAsync(x => x.Id == id);
        }

        public void Update(Expense expense)
        {
            _dbContext.Expenses.Update(expense);
        }
    }
}
