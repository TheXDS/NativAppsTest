using NativApps.Core.Models;

namespace NativApps.Core.Services;

/// <summary>
/// Defines a set of members to be implemented by a type that exposes methods
/// to read and write products in a data repository.
/// </summary>
public interface IProductRepository : IDisposable
{
    /// <summary>
    /// Adds the product into the repository data store.
    /// </summary>
    /// <param name="product">Product to add.</param>
    /// <returns></returns>
    public int Create(Product product);

    /// <summary>
    /// Reads a product with the specified Id from the data store.
    /// </summary>
    /// <param name="productId">Product Id to read.</param>
    /// <returns>
    /// A product with the specified Id, or <see langword="null"/> if no such
    /// product exists.
    /// </returns>
    public Product? Read(int productId) => GetAll().SingleOrDefault(p => p.ProductId == productId);

    /// <summary>
    /// Updates a product already present in the data store.
    /// </summary>
    /// <param name="data"></param>
    public void Update(Product data);

    /// <summary>
    /// Deletes a product with the specified Id from the data store.
    /// </summary>
    /// <param name="productId"></param>
    public void Delete(int productId);

    /// <summary>
    /// Gets a queryable object that may be used to construct queries into the
    /// data store.
    /// </summary>
    /// <returns>
    /// A queryable object that may be used to construct queries into the data
    /// store.
    /// </returns>
    public IQueryable<Product> GetAll();
}