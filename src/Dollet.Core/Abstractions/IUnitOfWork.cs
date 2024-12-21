using Dollet.Core.Abstractions.Repositories;
using Dollet.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Dollet.Core.Abstractions
{
    public interface IUnitOfWork
    {
        IAccountRepository AccountRepository { get; }
        IAppDataRepository AppDataRepository { get; }
        ICategoryRepository CategoryRepository { get; }
        ICurrencyRepository CurrencyRepository{ get; }
        IExpensesRepository ExpensesRepository { get; }
        IIncomesRepository  IncomesRepository { get; }
        IUserRepository UserRepository { get; }
        IAccountCategoryRepository AccountCategoryRepository { get; }
        Task<bool> CommitAsync();
        void SetApplicationContext(Users currentUser);
        Users GetApplicationContext();
        DbContext GetDbContext();
    }
}