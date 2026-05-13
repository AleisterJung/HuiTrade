using HuiTrade.Application.DTOs.User;
using Microsoft.AspNetCore.Mvc;
using HuiTrade.Application.Services.UserService;

namespace HuiTrade.Api.Controllers;

[ApiController]
[Route("api/users")]
public class UserController : ControllerBase
{
    private readonly UserService _userService;

    public UserController(UserService userService)
    {
        _userService = userService;
    }

    // 注册
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterUserRequest request)
    {
        var userId = await _userService.RegisterUserAsync(
            request.Username,
            request.Password,
            request.Nickname
        );

        return Ok(new
        {
            UserId = userId,
            Message = "注册成功"
        });
    }

    
}