using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HuiTrade.Domain.Aggregates
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(Guid id);

        Task<User?> GetByUsernameAsync(string username);
        Task AddAsync(User user);
       
      
    }
}
