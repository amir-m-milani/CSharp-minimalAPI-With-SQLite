using Microsoft.OpenApi.Models;
using Athenticate.Database;
using Microsoft.EntityFrameworkCore;
using Athenticate.Dto;
using Athenticate.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("Data Source=Database.db")));
builder.Services.AddScoped<UserService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "API",
        Description = "For Fun",
        Version = "v1"
    });
});

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "API");
    });
}


app.MapPost("/user/add", async (RegisterDto registerDto, UserService userService) =>
{
    var user = await userService.GetUserByEmailAsync(registerDto.Email);
    if (user != null)
    {
        return Results.BadRequest("User with this email already exists.");
    }

    var result = await userService.AddUser(registerDto);
    if (result)
    {
        return Results.Ok("User added successfully.");
    }

    return Results.BadRequest("Failed to add user.");
});

app.Run();

