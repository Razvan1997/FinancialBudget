using Dollet.Core.Abstractions.Repositories;
using Dollet.Core.Entities;
using Dollet.Core.Enums;
using Microsoft.EntityFrameworkCore;

namespace Dollet.Infrastructure.DAL.Repositories
{
    internal class CategoryRepository(DolletDbContext dbContext) : ICategoryRepository
    {
        private readonly DolletDbContext _dbContext = dbContext;

        public void Add(Category category)
        {
            _dbContext.Categories.Add(category);
        }

        public void AddMany(IEnumerable<Category> categories)
        {
            _dbContext.Categories.AddRange(categories);
        }

        public async Task<bool> AnyAsync()
        {
            return await _dbContext.Categories.AnyAsync();
        }

        public async Task<IEnumerable<Category>> GetAllAsync(CategoryType categoryType = CategoryType.Expense)
        {
            return await _dbContext.Categories
                .Where(c => c.Type == categoryType)
                .OrderBy(c => c.IndexOrder)
                .ToListAsync();
        }

        public async Task<Category> GetCategoryByIdAsync(int id)
        {
            var category = await _dbContext.Categories
                .FirstOrDefaultAsync(c => c.Id == id);

            if (category == null)
            {
                throw new KeyNotFoundException($"Categoria cu ID-ul {id} nu a fost găsită.");
            }

            return category;
        }

        public void UpdateMany(IEnumerable<Category> categories)
        {
            foreach (var category in categories)
            {
                _dbContext.Entry(category).State = EntityState.Modified;
            }
        }
    }
}
