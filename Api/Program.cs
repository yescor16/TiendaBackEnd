using Api.Errors;
using Api.Extensions;
using Api.Helpers;
using Api.Middleware;
using Core.Interfaces;
using Infrastructure.Data;
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
builder.Services.AddDbContext<StoreContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("dbConnection"));
});



//Extensiones
builder.Services.AddAplicationServices(builder.Configuration);
builder.Services.AddSwaggerDocumentation();




var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    
    var loggerFactory = scope.ServiceProvider.GetRequiredService<ILoggerFactory>();
	try
	{
        var context = scope.ServiceProvider.GetRequiredService<StoreContext>();
        await context.Database.MigrateAsync();
        await StorContextSeed.SeedAsync(context, loggerFactory);
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

app.UseAuthorization();
app.UseStaticFiles();
app.MapControllers();

app.Run();
