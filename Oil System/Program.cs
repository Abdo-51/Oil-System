using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Oil_System.Helper;
using Oil_System.Helper.Middleware;
using Oil_System.Repository.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.injectservices(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
//{
app.UseSwagger();
app.UseSwaggerUI();

//Apply pending migrations at startup
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.Migrate();

    //Add default roles if they don't exist
    var RoleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    if (!await RoleManager.RoleExistsAsync("Admin"))
    {
        await RoleManager.CreateAsync(new IdentityRole("Admin"));
    }
    if (!await RoleManager.RoleExistsAsync("User"))
    {
        await RoleManager.CreateAsync(new IdentityRole("User"));
    }
}

//}

app.UseMiddleware<CustomMiddleware>();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
