using Dollet.Core.Abstractions.Repositories;
using Dollet.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Dollet.Infrastructure.DAL.Repositories
{
    internal class AppDataRepository(DolletDbContext dbContext) : IAppDataRepository
    {
        private readonly DolletDbContext _dbContext = dbContext;

        public async Task<AppData?> GetAsync()
        {
            return await _dbContext.AppData.FirstOrDefaultAsync();
        }

        public async Task<bool> UpdateAsync(AppData appData)
        {
            _dbContext.AppData.Update(appData);
            return await _dbContext.SaveChangesAsync() > 0;
        }
    }
}
