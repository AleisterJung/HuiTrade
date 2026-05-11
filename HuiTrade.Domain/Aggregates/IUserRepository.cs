using System;
using System.Collections.Generic;
using System.Text;

namespace HuiTrade.Domain.Aggregates
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(Guid id);

        Task<User?> GetByUsernameAsync(string username);
        Task AddAsync(User user);
        Task UpdateAsync(User user);

        // 统一提交所有更改到数据库
        Task<int> SaveChangesAsync();
      
    }
}
