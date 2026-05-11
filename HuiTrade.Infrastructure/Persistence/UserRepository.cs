using HuiTrade.Domain.Aggregates;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace HuiTrade.Infrastructure.Persistence
{
    public class UserRepository : IUserRepository
    {
        private readonly HuiTradeDbContext _context;

        public UserRepository(HuiTradeDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetByIdAsync(Guid id)
        {
            return await _context.Users
                .Include(u => u.UserWallet)
                .Include(u => u.Addresses)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            return await _context.Users
            .Include(u => u.UserWallet)
            .FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
        }

        public async Task UpdateAsync(User user)
        {
            // EF Core 的 ChangeTracker 会自动跟踪实体的属性变化
            // 这里显式调用 Update 主要是为了确保实体被标记为 Modified
            _context.Users.Update(user);
            await Task.CompletedTask;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
