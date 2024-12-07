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
    internal class UsersRepository(DolletDbContext dbContext) : IUserRepository
    {
        private readonly DbSet<Users> _users = dbContext.Users;
        public void Add(Users user)
        {
            _users.Add(user);
        }

        public void AddMany(IEnumerable<Users> users)
        {
            dbContext.Users.AddRange(users);
        }

        public async Task<bool> AnyAsync()
        {
            return await dbContext.Users.AnyAsync();
        }

        public async Task<Users?> GetAsync(int id)
        {
            return await _users.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Users?> GetByUsernameAndPasswordAsync(string username, string password)
        {
            return await _users.FirstOrDefaultAsync(user => user.Name == username && user.Password == password);
        }
    }
}
