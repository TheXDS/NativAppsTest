using NativApps.Core.Models;
using ProductDto = NativApps.Core.Models.Dto.ProductDto;

namespace NativApps.Core.Services;

/// <summary>
/// Implements a product service that reads and writes data in an
/// <see cref="IProductRepository"/> instance.
/// </summary>
public class ProductService : IProductService
{
    private static readonly Func<ProductDto, bool>[] postContracts =
    {
        p => p is null,
        p => string.IsNullOrWhiteSpace(p.Name),
        p => string.IsNullOrWhiteSpace(p.Description),
        p => string.IsNullOrWhiteSpace(p.Category),
        p => p.Price is null,
        p => p.InitialQuantity is null,
    };
    private readonly IProductRepository repository;

    /// <summary>
    /// Initializes a new instance of the <see cref="ProductService"/> class.
    /// </summary>
    /// <param name="repository">
    /// Repository instance to use when retrieving and/or saving data.
    /// </param>
    public ProductService(IProductRepository repository)
    {
        this.repository = repository;
    }

    /// <inheritdoc/>
    public ServiceResult<int> Create(ProductDto product)
    {
        if (postContracts.Any(p => p.Invoke(product)))
        {
            return new ServiceResult<int>(false, "The request to create a new product is missing some data.");
        }
        Product newProduct = new()
        {
            Name = product.Name!,
            Description = product.Description!,
            Category = product.Category!,
            Price = product.Price!.Value,
            InitialQuantity = product.InitialQuantity!.Value,
        };
        try
        {
            var result = repository.Create(newProduct);
            return result > 0
                ? result
                : new ServiceResult<int>(false, "Error while creating the new product.");
        }
        catch (Exception ex)
        {
            return new ServiceResult<int>(false, ex.Message);
        }
    }

    /// <inheritdoc/>
    public ServiceResult Delete(int productId)
    {
        if (Read(productId) is { Success: true })
        {
            repository.Delete(productId);
            return true;
        }
        else
        {
            return new ServiceResult(false, "Product not found");
        };
    }

    /// <inheritdoc/>
    public ServiceResult<Product> Read(int productId)
    {
        return repository.Read(productId) is { } product
            ? product
            : new ServiceResult<Product>(false, "Product not found");
    }

    /// <inheritdoc/>
    public ServiceResult Update(int productId, ProductDto data)
    {
        if (Read(productId) is { Success: true, Result: { } updatedProduct })
        {
            bool changesMade = false;
            void SetIfNotNull<T>(T? value, Func<Product, T> currentValue, Action<Product, T> setter) where T : notnull
            {
                if (value is not null && !value.Equals(currentValue.Invoke(updatedProduct)))
                { 
                    changesMade = true;
                    setter.Invoke(updatedProduct, value);
                }
            }

#pragma warning disable CS8714
            SetIfNotNull(data.Name, p => p.Name, (p, v) => p.Name = v);
            SetIfNotNull(data.Description, p => p.Description, (p, v) => p.Description = v);
            SetIfNotNull(data.Category, p => p.Category, (p, v) => p.Category = v);
            SetIfNotNull(data.Price, p => p.Price, (p, v) => p.Price = v!.Value);
            SetIfNotNull(data.InitialQuantity, p => p.InitialQuantity, (p, v) => p.InitialQuantity = v!.Value);
#pragma warning restore CS8714

            if (changesMade)
            {
                repository.Update(updatedProduct);
            }
            return new ServiceResult(true, changesMade ? null : "No changes were made");
        }
        else
        { 
            return new ServiceResult(false, "Product not found");
        };
    }

    /// <inheritdoc/>
    public IEnumerable<Product> GetByName(string name)
    {
        return repository
            .GetAll()
            .Where(p => p.Name.Contains(name, StringComparison.InvariantCultureIgnoreCase));
    }

    /// <inheritdoc/>
    public IEnumerable<Product> GetByPriceRange(decimal minPrice, decimal maxPrice)
    {
        return repository
            .GetAll()
            .Where(p => p.Price >= minPrice && p.Price <= maxPrice);
    }

    /// <inheritdoc/>
    public IEnumerable<Product> GetAll()
    {
        return repository.GetAll();
    }
}
