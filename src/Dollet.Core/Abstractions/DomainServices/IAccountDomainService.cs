using Dollet.Core.Entities;

namespace Dollet.Core.Abstractions.DomainServices
{
    public interface IAccountDomainService
    {
        Task<bool> CreateAsync(Account newAccount, bool isDefault = false);
        Task<bool> EditAsync(Account existingAccount, bool isDefault = false);
        Task<bool> DeleteAsync(Account account, bool includeTransactions = false);
        Task<int?> CreateAndGetIdAsync(Account newAccount, bool isDefault = false);
        Task<bool> AccountAlreadyExist(Account newAccount);
    }
}
