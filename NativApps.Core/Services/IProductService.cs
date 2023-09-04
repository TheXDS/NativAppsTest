using NativApps.Core.Models;
using ProductDto = NativApps.Core.Models.Dto.ProductDto;

namespace NativApps.Core.Services;

/// <summary>
/// Defines a set of members to be implemented by a type that can store and
/// retrieve products.
/// </summary>
public interface IProductService
{
    /// <summary>
    /// Creates a new product.
    /// </summary>
    /// <param name="product">Product DTO with the data to insert.</param>
    /// <returns>
    /// A <see cref="ServiceResult{T}"/> with the product id of the newly
    /// created product.
    /// </returns>
    public ServiceResult<int> Create(ProductDto product);

    /// <summary>
    /// Gets an existing product with the specified id.
    /// </summary>
    /// <param name="productId">Product Id to get.</param>
    /// <returns>
    /// A <see cref="ServiceResult{T}"/> with either the product that has the
    /// specified product id or <see langword="null"/> if no such product has
    /// been found.
    /// </returns>
    public ServiceResult<Product> Read(int productId);

    /// <summary>
    /// Updates an existing product with the specified data.
    /// </summary>
    /// <param name="productId">Product Id to update.</param>
    /// <param name="data">DTO with the new data to be set.</param>
    /// <returns>
    /// A <see cref="ServiceResult"/> that indicates either success or failure
    /// to perform the operation.
    /// </returns>
    public ServiceResult Update(int productId, ProductDto data);

    /// <summary>
    /// Deletes the product with the specified product id.
    /// </summary>
    /// <param name="productId">Product Id to delete.</param>
    /// <returns>
    /// A <see cref="ServiceResult"/> that indicates either success or failure
    /// to perform the operation.
    /// </returns>
    public ServiceResult Delete(int productId);

    /// <summary>
    /// Gets all products that contain the specified string in its name.
    /// </summary>
    /// <param name="name">Name to search for.</param>
    /// <returns>
    /// A collection of all products whose names include the specified string.
    /// </returns>
    IEnumerable<Product> GetByName(string name);

    /// <summary>
    /// Gets all the products that fall in the specified range of prices.
    /// </summary>
    /// <param name="minPrice">Minimum price to include.</param>
    /// <param name="maxPrice">Maximum price to include.</param>
    /// <returns>
    /// A collection of all products whose prices fall in the specified range.
    /// </returns>
    IEnumerable<Product> GetByPriceRange(decimal minPrice, decimal maxPrice);

    /// <summary>
    /// Enumerates all products.
    /// </summary>
    /// <returns>A collection of all products</returns>
    IEnumerable<Product> GetAll();
}
