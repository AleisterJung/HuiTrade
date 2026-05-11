using Microsoft.Extensions.Configuration;  
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using HuiTrade.Infrastructure.Persistence;
using HuiTrade.Domain.Aggregates;

namespace HuiTrade.Infrastructure;

public static class DependencyInjection
{
    // 修改参数类型为 IConfiguration
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // 从 appsettings.json 中读取 ConnectionStrings:DefaultConnection
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<HuiTradeDbContext>(options =>
            options.UseNpgsql(connectionString));

        services.AddScoped<IUserRepository, UserRepository>();

        return services;
    }
}