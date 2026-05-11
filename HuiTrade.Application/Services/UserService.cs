using HuiTrade.Domain.Aggregates;
using System;
using System.Collections.Generic;
using System.Text;

namespace HuiTrade.Application.Services
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Guid> RegisterUserAsync(string username, string password, string nickname)
        {

            var user = User.Register(username, password, nickname);

            await _userRepository.AddAsync(user);
            await _userRepository.SaveChangesAsync();

            return user.Id;
        }
        public async Task AddUserAddressAsync(Guid userId, string receiver, string phone, string detail, bool isDefault)
        {
            var user = await _userRepository.GetByIdAsync(userId);

            if (user == null) throw new InvalidOperationException("用户不存在");

            user.AddAddress(receiver, phone, detail, isDefault);

             
            await _userRepository.SaveChangesAsync();
        }
    }


}
