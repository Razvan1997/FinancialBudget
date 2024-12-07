using Dollet.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dollet.Core.Abstractions.Repositories
{
    public interface IAccountCategoryRepository
    {
        /// <summary>
        /// Adaugă o categorie la un cont specific.
        /// </summary>
        /// <param name="accountId">Id-ul contului.</param>
        /// <param name="categoryId">Id-ul categoriei.</param>
        /// <returns></returns>
        Task AddCategoryToAccountAsync(int accountId, int categoryId, decimal budget);

        /// <summary>
        /// Elimină o categorie dintr-un cont specific.
        /// </summary>
        /// <param name="accountId">Id-ul contului.</param>
        /// <param name="categoryId">Id-ul categoriei.</param>
        /// <returns></returns>
        Task RemoveCategoryFromAccountAsync(int accountId, int categoryId);

        /// <summary>
        /// Obține toate categoriile asociate unui cont specific.
        /// </summary>
        /// <param name="accountId">Id-ul contului.</param>
        /// <returns>O listă de categorii asociate contului.</returns>
        Task<IEnumerable<AccountCategory>> GetCategoriesByAccountIdAsync(int accountId);

        /// <summary>
        /// Actualizează toate categoriile asociate unui cont.
        /// </summary>
        /// <param name="accountId">Id-ul contului.</param>
        /// <param name="categories">Lista de categorii actualizate.</param>
        /// <returns></returns>
        Task UpdateCategoriesAtAccountAsync(int accountId, IEnumerable<int> categoryIds);
    }
}
