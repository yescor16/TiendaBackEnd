using Api.Errors;
using Api.Extensions;
using Api.Helpers;
using Api.Middleware;
using Core.Entities.Identity;
using Core.Interfaces;
using Infrastructure.Data;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using System.ComponentModel;
using System.Runtime.InteropServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCors();



builder.Services.AddAutoMapper(typeof(MappingProfiles));

//Extensiones
builder.Services.AddIdentityServices(builder.Configuration);
builder.Services.AddAplicationServices(builder.Configuration);
builder.Services.AddSwaggerDocumentation();




var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    
    
    var loggerFactory = scope.ServiceProvider.GetRequiredService<ILoggerFactory>();
	try
	{
        var services = scope.ServiceProvider;

        var context = services.GetRequiredService<StoreContext>();
        await context.Database.MigrateAsync();
        await StorContextSeed.SeedAsync(context, loggerFactory);


        var userManager = services.GetRequiredService<UserManager<AppUser>>();

        var identityContext = services.GetRequiredService<AppIdentityDbContext>();
        await identityContext.Database.MigrateAsync();
        await AppIdentityDbContextSeed.SeedUserAsync(userManager);
    }
	catch (Exception ex)
	{
        var logger = loggerFactory.CreateLogger<Program>();
        logger.LogError(ex,"An Error ocurred during Migration");
	}
    
}
//Extension
app.UseSwaggerDocumentation();

app.UseMiddleware<ExceptionMiddleware>();
app.UseStatusCodePagesWithReExecute("/errors/{0}");


app.UseCors(builder => builder
     .AllowAnyOrigin()
     .AllowAnyMethod()
     .AllowAnyHeader());

app.UseRouting();



app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.UseStaticFiles();
app.MapControllers();

app.Run();
