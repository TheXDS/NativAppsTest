using Microsoft.EntityFrameworkCore;
using NativApps.Core.Models;
using NativApps.Core.Services;
using NativAppsApi.Data;

namespace NativAppsApi;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddDbContext<NativAppsApiContext>(options =>
        {
            options.UseInMemoryDatabase(nameof(NativAppsApiContext));
        });
        builder.Services.AddTransient<IProductRepository, ProductRepository>();
        builder.Services.AddTransient<IProductService, ProductService>();
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddAuthentication("Bearer").AddJwtBearer().AddJwtBearer("LocalAuthIssuer");
        builder.Services.AddSwaggerGen();
        var app = builder.Build();
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseRouting();
        app.UseAuthorization();
        app.MapControllers();
        await app.RunAsync();
    }
}