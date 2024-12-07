using Dollet.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dollet.Core.Abstractions.Repositories
{
    public interface IUserRepository
    {
        Task<Users?> GetAsync(int id);
        void Add(Users users);
        Task<bool> AnyAsync();
        void AddMany(IEnumerable<Users> users);
        Task<Users?> GetByUsernameAndPasswordAsync(string username, string password);
    }
}
