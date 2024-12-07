using Dollet.Core.Entities;

namespace Dollet.Core.Abstractions.Repositories
{
    public interface IAppDataRepository
    {
        Task<AppData?> GetAsync();
        Task<bool> UpdateAsync(AppData appData);
    }
}