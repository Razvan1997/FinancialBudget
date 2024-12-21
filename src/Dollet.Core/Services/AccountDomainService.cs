using Dollet.Core.Abstractions;
using Dollet.Core.Abstractions.DomainServices;
using Dollet.Core.Abstractions.Repositories;
using Dollet.Core.Entities;
using Dollet.Core.Exceptions;
using Dollet.Core.ExtensionMethods;
using Microsoft.EntityFrameworkCore;

namespace Dollet.Core.Services
{
    internal class AccountDomainService(IUnitOfWork unitOfWork) : IAccountDomainService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IAccountRepository _accountRepository = unitOfWork.AccountRepository;
        private readonly DbContext _dbContext = unitOfWork.GetDbContext();

        public async Task<bool> CreateAsync(Account newAccount, bool isDefault = false)
        {
            newAccount.Name.ToFirstUpper();
            newAccount.Description.ToFirstUpper();

            var defaultAccount = await _accountRepository.GetDefaultAsync();

            if (defaultAccount is null)
            {
                newAccount.SetAsDefault();
            }
            else if (isDefault)
            {
                defaultAccount?.UnsetAsDefault();

                if (defaultAccount is not null)
                    _accountRepository.Update(defaultAccount);
                newAccount.SetAsDefault();
            }

            _accountRepository.Add(newAccount);
            return await _unitOfWork.CommitAsync();
        }

        public async Task<int?> CreateAndGetIdAsync(Account newAccount, bool isDefault = false)
        {
            newAccount.Name.ToFirstUpper();
            newAccount.Description.ToFirstUpper();

            var defaultAccount = await _accountRepository.GetDefaultAsync();

            if (defaultAccount is null)
            {
                newAccount.SetAsDefault();
            }
            else if (isDefault)
            {
                defaultAccount?.UnsetAsDefault();

                if (defaultAccount is not null)
                    _accountRepository.Update(defaultAccount);

                newAccount.SetAsDefault();
            }

            _accountRepository.Add(newAccount);

            var success = await _unitOfWork.CommitAsync();

            return success ? newAccount.Id : null;
        }

        public async Task<bool> EditAsync(Account existingAccount, bool isDefault = false)
        {
            ArgumentNullException.ThrowIfNull(existingAccount);

            existingAccount.Name.ToFirstUpper();
            existingAccount.Description.ToFirstUpper();

            // Caută entitatea deja urmărită în DbContext
            var trackedAccount = await _accountRepository.GetByIdAsync(existingAccount.Id);

            if (trackedAccount == null)
            {
                throw new InvalidOperationException("Account not found.");
            }

            // Actualizează entitatea urmărită cu noile valori
            _dbContext.Entry(trackedAccount).CurrentValues.SetValues(existingAccount);

            // Logica pentru schimbarea contului implicit
            var defaultAccount = await _accountRepository.GetDefaultAsync();

            if (isDefault && !trackedAccount.IsDefault)
            {
                if (defaultAccount != null && defaultAccount.Id != trackedAccount.Id)
                {
                    defaultAccount.UnsetAsDefault();
                    _accountRepository.Update(defaultAccount);
                }
                trackedAccount.SetAsDefault();
            }
            else if (!isDefault && trackedAccount.IsDefault)
            {
                if (defaultAccount == null || defaultAccount.Id == trackedAccount.Id)
                {
                    throw new RequireOneDefaultAccountException();
                }
                trackedAccount.UnsetAsDefault();
            }

            // Updatează entitatea urmărită (nu atașăm `existingAccount` direct)
            _accountRepository.Update(trackedAccount);

            return await _unitOfWork.CommitAsync();
        }

        public async Task<bool> DeleteAsync(Account account, bool includeTransactions = false)
        {
            ArgumentNullException.ThrowIfNull(account);

            if (account.IsDefault)
            {
                throw new CannotDeleteDefaultAccountException();
            }

            _accountRepository.Delete(account);

            if (includeTransactions)
            {
                await _unitOfWork.ExpensesRepository.DeleteAllAsync(account.Id);
                await _unitOfWork.IncomesRepository.DeleteAllAsync(account.Id);
            }

            return await _unitOfWork.CommitAsync();
        }

        public async Task<bool> AccountAlreadyExist(Account newAccount)
        {
            var account = await _accountRepository.GetAsyncByUserAndPass(newAccount.Username, newAccount.Password);
            return account.Any();
        }
    }
}