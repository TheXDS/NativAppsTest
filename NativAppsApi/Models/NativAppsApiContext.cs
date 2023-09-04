using Microsoft.EntityFrameworkCore;

namespace NativApps.Core.Models;

public class NativAppsApiContext : DbContext
{
    public NativAppsApiContext()
    {
    }

    public NativAppsApiContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }
}