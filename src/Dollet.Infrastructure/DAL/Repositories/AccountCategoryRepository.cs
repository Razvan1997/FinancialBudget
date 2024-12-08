using Dollet.Core.Abstractions.Repositories;
using Dollet.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dollet.Infrastructure.DAL.Repositories
{
    internal class AccountCategoryRepository(DolletDbContext dbContext) : IAccountCategoryRepository
    {
        private readonly DbSet<AccountCategory> _accountCategories = dbContext.AccountCategories;

        public async Task AddCategoryToAccountAsync(int accountId, int categoryId, decimal budget)
        {
            var accountCategory = new AccountCategory
            {
                AccountId = accountId,
                CategoryId = categoryId,
                Budget = budget
            };

            await _accountCategories.AddAsync(accountCategory);
        }

        public async Task RemoveCategoryFromAccountAsync(int accountId, int categoryId)
        {
            var accountCategory = await _accountCategories
                .FirstOrDefaultAsync(ac => ac.AccountId == accountId && ac.CategoryId == categoryId);

            if (accountCategory != null)
            {
                _accountCategories.Remove(accountCategory);
            }
        }

        public async Task<IEnumerable<AccountCategory>> GetCategoriesByAccountIdAsync(int accountId)
        {
            return await _accountCategories
                .Where(ac => ac.AccountId == accountId)
                .ToListAsync();
        }

        public async Task UpdateCategoriesAtAccountAsync(int accountId, IEnumerable<int> categoryIds)
        {
            var existingRelations = _accountCategories
                .Where(ac => ac.AccountId == accountId);

            _accountCategories.RemoveRange(existingRelations);

            var newRelations = categoryIds.Select(categoryId => new AccountCategory
            {
                AccountId = accountId,
                CategoryId = categoryId
            });

            await _accountCategories.AddRangeAsync(newRelations);
        }

        public async Task<AccountCategory?> GetCategoryByAccountIdAndCategoryIdAsync(int accountId, int categoryId)
        {
            return await _accountCategories
                .FirstOrDefaultAsync(ac => ac.AccountId == accountId && ac.CategoryId == categoryId);
        }
    }
}
