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

        PopulateTestDb(app.Services, 20);

        await app.RunAsync();
    }

    private static void PopulateTestDb(IServiceProvider serviceCollection, int qty)
    {
        using var svc = serviceCollection.GetRequiredService<IProductRepository>();

        foreach (var product in Enumerable.Range(1, qty).Select(GetFullProduct))
        {
            svc.Create(product);
        }
    }

    private static readonly Random _rnd = new();

    private static Product GetFullProduct(int id)
    {
        return new Product()
        {
            ProductId = id,
            Category = $"CategoryValue {id % 4}",
            Description = $"DescriptionValue {id} DescriptionValue {id}. Lorem ipsum dolot sit amet consectur viamos laude.",
            InitialQuantity = _rnd.Next(25, 100),
            Name = $"NameValue {id}",
            Price = _rnd.Next(2, 20) * 50,
        };
    }
}