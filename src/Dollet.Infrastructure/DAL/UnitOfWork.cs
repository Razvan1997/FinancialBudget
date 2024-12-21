using Dollet.Core.Abstractions;
using Dollet.Core.Abstractions.Repositories;
using Dollet.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Dollet.Infrastructure.DAL
{
    internal class UnitOfWork(DolletDbContext dbContext, Lazy<IAccountRepository> accountRepository,
        Lazy<IAppDataRepository> appDataRepository, Lazy<ICategoryRepository> categoryRepository,
        Lazy<ICurrencyRepository> currencyRepository, Lazy<IExpensesRepository> expensesRepository,
        Lazy<IIncomesRepository> incomesRepository, Lazy<IUserRepository> userRepository,
        Lazy<IAccountCategoryRepository> accountCategoryRepository) : IUnitOfWork
    {
        private readonly DolletDbContext _dbContext = dbContext;
        private static Users _currentUserContext;
        private readonly Lazy<IAccountRepository> _accountRepository = accountRepository;
        private readonly Lazy<IAppDataRepository> _appDataRepository = appDataRepository;
        private readonly Lazy<ICategoryRepository> _categoryRepository = categoryRepository;
        private readonly Lazy<ICurrencyRepository> _currencyRepository = currencyRepository;
        private readonly Lazy<IExpensesRepository> _expensesRepository = expensesRepository;
        private readonly Lazy<IIncomesRepository> _incomesRepository = incomesRepository;
        private readonly Lazy<IUserRepository> _userRepository = userRepository;
        private readonly Lazy<IAccountCategoryRepository> _accountCategoryRepository = accountCategoryRepository;
        public IAccountRepository AccountRepository => _accountRepository.Value;
        public IAppDataRepository AppDataRepository => _appDataRepository.Value;
        public ICategoryRepository CategoryRepository => _categoryRepository.Value;
        public ICurrencyRepository CurrencyRepository => _currencyRepository.Value;
        public IExpensesRepository ExpensesRepository => _expensesRepository.Value;
        public IIncomesRepository IncomesRepository => _incomesRepository.Value;
        public IUserRepository UserRepository => _userRepository.Value;
        public IAccountCategoryRepository AccountCategoryRepository => _accountCategoryRepository.Value;

        public async Task<bool> CommitAsync()
        {
            var saved = await _dbContext.SaveChangesAsync() > 0;

            _dbContext.ChangeTracker.Clear();

            return saved;
        }

        public static Users CurrentUserContext
        {
            get => _currentUserContext;
            private set => _currentUserContext = value;
        }

        public DbContext GetDbContext()
        {
            return _dbContext;
        }

        public void SetApplicationContext(Users currentUser)
        {
            CurrentUserContext = currentUser;
        }

        public Users GetApplicationContext()
        {
            return CurrentUserContext;
        }
    }
}