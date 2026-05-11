using Microsoft.AspNetCore.Mvc;
using HuiTrade.Application.Services;

namespace HuiTrade.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly UserService _userService;

    public UserController(UserService userService)
    {
        _userService = userService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(string username,string password, string nickname)
    {
        try
        {
            var userId = await _userService.RegisterUserAsync(username, password, nickname);
            return Ok(new { id = userId, message = "注册成功" });
        }
        catch (Exception ex)
        {
            // 获取最底层的 PostgresException
            var message = ex.InnerException?.Message ?? ex.Message;
            // 在控制台打印完整的堆栈信息，方便查找具体是哪个属性出错了
            Console.WriteLine($"[DB ERROR]: {ex.ToString()}");
            return BadRequest(new { error = "数据库保存失败", detail = message });
        }
    }
}