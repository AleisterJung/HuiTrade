using System;
using System.Threading.Tasks;
using HuiTrade.Domain.Aggregates;

namespace HuiTrade.Application.Services.UserService;

public class UserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Guid> RegisterUserAsync(
        string username,
        string password,
        string nickname)
    {
        // 1. 检查用户是否存在
        var exist = await _userRepository.GetByUsernameAsync(username);
        if (exist != null)
            throw new InvalidOperationException("用户名已存在");

        // 2. 加密密码
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(password);

        // 3. 创建用户
        var user = User.Register(username, passwordHash, nickname);

        // 4. 保存（注意：Repository 内部必须调用 SaveChanges）
        await _userRepository.AddAsync(user);

        return user.Id;
    }
}