using Dollet.Core.Entities;
using Dollet.Core.Enums;

namespace Dollet.Core.Abstractions.Repositories
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetAllAsync(CategoryType categoryType = CategoryType.Expense);
        void Add(Category category);
        void AddMany(IEnumerable<Category> categories);
        void UpdateMany(IEnumerable<Category> categories);
        Task<bool> AnyAsync();
        Task<Category> GetCategoryByIdAsync(int id);
    }
}
