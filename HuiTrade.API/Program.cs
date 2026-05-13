using HuiTrade.Infrastructure;
using HuiTrade.Application.Services;
using HuiTrade.Application.Services.UserService;

var builder = WebApplication.CreateBuilder(args);

  
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<UserService>();
 
builder.Services.AddControllers();

 
var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "HuiTrade API v1");
    });
}

app.UseHttpsRedirection();
 
app.MapControllers();

app.Run();