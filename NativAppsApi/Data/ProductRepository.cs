using NativApps.Core.Models;
using NativApps.Core.Services;

namespace NativAppsApi.Data
{
    public class ProductRepository : IDisposable, IProductRepository
    {
        private readonly NativAppsApiContext context;
        private bool disposedValue;

        public ProductRepository(NativAppsApiContext context)
        {
            this.context = context;
        }

        public int Create(Product product)
        {
            context.Products.Add(product);
            context.SaveChanges();
            return product.ProductId;
        }

        public void Delete(int productId)
        {
            if (context.Products.Find(productId) is { } product)
            {
                context.Products.Remove(product);
                context.SaveChanges();
            }
        }

        public IQueryable<Product> GetAll()
        {
            return context.Products;
        }

        public void Update(Product data)
        {
            context.Update(data);
            context.SaveChanges();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    context.Dispose();
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
