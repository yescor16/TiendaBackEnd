using Api.Helpers;
using Api.Middleware;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.Runtime.InteropServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddAutoMapper(typeof(MappingProfiles));
builder.Services.AddScoped<IProductRepository,ProductRepository>();
builder.Services.AddScoped(typeof( IGenericRepository<>),typeof(GenericRepository<>));
builder.Services.AddDbContext<StoreContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("dbConnection"));
});
builder.Services.AddScoped<StoreContext>();
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

// Configure the HTTP request pipeline.

app.UseMiddleware<ExceptionMiddleware>();
app.UseStatusCodePagesWithReExecute("/errors/{0}");

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseStaticFiles();
app.MapControllers();

app.Run();
